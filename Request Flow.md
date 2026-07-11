# CortexaGrad Request Flow

This file explains how a request moves through the `CortexaGrad` backend.

## High-Level Architecture

The project follows a layered structure:

- `Cortexa.Api`
- `Cortexa.Application`
- `Cortexa.Domain`
- `Cortexa.Infrastructure`

The request path is:

`Client -> API Middleware -> Controller -> MediatR -> Application Handler -> Repository/UnitOfWork -> DbContext -> SQL Server -> Response`

## What Each Layer Does

### 1. `Cortexa.Api`

Responsible for HTTP concerns:

- routing
- authentication and authorization
- middleware
- controllers
- SignalR hubs

Important startup file:

- `Cortexa.Api/Program.cs`

Main registrations:

- `builder.Services.AddApplication();`
- `builder.Services.AddInfrastructure(builder.Configuration);`
- `builder.Services.AddApiServices();`

Main runtime pipeline:

1. `ExceptionMiddleware`
2. `UseCors`
3. `UseAuthentication`
4. `UseAuthorization`
5. `MapControllers`

So every request enters through the API project first.

### 2. `Cortexa.Application`

Responsible for use-case logic:

- commands
- queries
- handlers
- DTOs
- validators
- mapping

This layer uses `MediatR`.

The controller does not directly call repositories or EF Core.
Instead, it sends a command or query through MediatR, and MediatR routes it to the correct handler.

### 3. `Cortexa.Domain`

Responsible for core business rules and entities:

- entities like `Patient`, `Admission`, `VitalSigns`
- enums
- domain exceptions
- domain services like NEWS score calculation

This is the pure business core.

### 4. `Cortexa.Infrastructure`

Responsible for implementation details:

- EF Core
- SQL Server
- Identity
- JWT
- repositories
- Unit of Work
- email service
- external AI integration

This is where interfaces from Application are actually implemented.

## Full Request Flow

When the frontend or Swagger sends a request, the flow is:

1. The request reaches ASP.NET Core in `Program.cs`.
2. Middleware runs first.
3. Authentication checks the JWT token if the endpoint is protected.
4. Authorization checks roles like `Doctor` or `Nurse`.
5. Routing sends the request to the matching controller action.
6. The controller creates or receives a command/query object.
7. The controller calls `Sender.Send(...)`.
8. MediatR finds the matching handler in the Application layer.
9. Validation behavior runs before the handler if a validator exists.
10. The handler performs the use case.
11. The handler uses repositories or `IUnitOfWork`.
12. Repositories talk to `CortexaDbContext`.
13. `DbContext.SaveChangesAsync()` applies audit fields, soft delete, and audit logs.
14. SQL Server stores or reads the data.
15. The result goes back to the handler.
16. The handler returns DTOs or IDs.
17. The controller returns HTTP response like `200 OK`, `201 Created`, `204 NoContent`, or `404 NotFound`.

## Real Example: Record Vital Signs

Endpoint:

- `POST /api/admissions/{admissionId}/vitals`

Files involved:

- `Cortexa.Api/Controllers/VitalSignsController.cs`
- `Cortexa.Application/Features/ClinicalData/Commands/AddCommands/RecordVitalsCommand.cs`
- `Cortexa.Infrastructure/Persistence/Repositories/Clinical/VitalSignsRepository.cs`
- `Cortexa.Infrastructure/Persistence/Repositories/UnitOfWork.cs`
- `Cortexa.Infrastructure/Persistence/CortexaDbContext.cs`

### Step-by-step

1. The request enters `VitalSignsController.Record`.
2. The controller takes `admissionId` from the route.
3. It assigns that route value into `RecordVitalsCommand.AdmissionId`.
4. The controller calls `await Sender.Send(command)`.
5. MediatR sends the command to `RecordVitalsCommandHandler`.
6. The handler creates a `VitalSigns` entity.
7. The handler calculates the NEWS score using `NewsCalculator`.
8. If risk is medium or high, it also creates an `Alert`.
9. It sends a SignalR notification through `INotificationService`.
10. It stores data through `_unitOfWork.VitalSigns.AddAsync(...)`.
11. It calls `_unitOfWork.SaveChangesAsync(...)`.
12. EF Core writes to the database.
13. The controller returns `201 Created` with the new record id.

## Read Example: Get Vitals History

Endpoint:

- `GET /api/admissions/{admissionId}/vitals`

Flow:

1. Request reaches `VitalSignsController.GetHistory`.
2. Controller sends `new GetVitalsHistoryQuery(admissionId)`.
3. MediatR calls `GetVitalsHistoryQueryHandler`.
4. Handler uses `_unitOfWork.VitalSigns.GetByAdmissionIdAsync(...)`.
5. Repository queries `CortexaDbContext.VitalSigns`.
6. Handler maps entities to `VitalSignsDto` using AutoMapper.
7. Controller returns `200 OK` with the result list.

This is the read pattern used across the project:

`Controller -> Query -> Handler -> Repository -> DbContext -> DTO -> Response`

## Why MediatR Is Important Here

MediatR is the center of your request flow.

Instead of this:

- Controller -> Service -> Repository

your project mainly uses:

- Controller -> MediatR -> Handler -> Repository

Benefits:

- controllers stay thin
- each use case is isolated
- commands and queries are separated
- validation can run in one place
- easier to test each handler independently

## Validation Flow

Validation is configured in the Application layer through a MediatR pipeline behavior.

That means:

1. controller sends command
2. MediatR runs validators
3. if validation fails, `ValidationException` is thrown
4. `ExceptionMiddleware` catches it
5. API returns `400 Bad Request` with validation details

So validation is not mainly inside controllers.
It is centralized in the pipeline.

## Exception Flow

Unhandled exceptions are caught by `ExceptionMiddleware`.

It converts exceptions into HTTP responses such as:

- `400 Bad Request`
- `401 Unauthorized`
- `404 Not Found`
- `409 Conflict`
- `500 Server Error`

This keeps controller code cleaner and gives a standard error response shape.

## Authentication and Current User

JWT authentication is configured in Infrastructure.

Protected endpoints use:

- `[Authorize]`
- `[Authorize(Roles = "Doctor,Nurse")]`

The current logged-in user is extracted by `CurrentUserService` from JWT claims.

That user data is later used in `CortexaDbContext.SaveChangesAsync()` for:

- `CreatedBy`
- `LastModifiedBy`
- `DeletedBy`
- audit logging

## Database Save Flow

When a handler calls `_unitOfWork.SaveChangesAsync()`:

1. UnitOfWork forwards the call to `CortexaDbContext`
2. `CortexaDbContext.SaveChangesAsync()` runs custom logic first
3. soft delete is applied
4. metadata fields are filled
5. audit logs are prepared
6. EF Core finally saves to SQL Server

So the real save flow is not just a direct insert/update.
There is extra behavior around every save.

## SignalR in Request Flow

Some requests also trigger real-time updates.

Example:

- recording vital signs with dangerous NEWS score

That flow becomes:

`HTTP request -> Handler -> Save Alert -> SignalR Notification -> Database save -> Response`

SignalR hubs are mapped in:

- `/hubs/alerts`
- `/hubs/monitoring`

## Summary

The backend is designed around thin controllers and Application handlers.

The most important idea to understand is:

- API receives the request
- MediatR routes the use case
- Application handler performs business logic
- Infrastructure handles persistence and auth
- Domain holds the core model and rules

If you understand that pattern, you can trace almost any endpoint in the repo.

## One Important Note

While tracing the project, I found one likely bug in:

- `Cortexa.Api/Controllers/VitalSignsController.cs`

In the `Update` action, the mismatch check compares `admissionId == command.Id`, which looks incorrect and likely reversed.

## Good Demo Path To Show Your Friend

If you want to explain the project quickly, show this order:

1. `Cortexa.Api/Program.cs`
2. `Cortexa.Api/Controllers/VitalSignsController.cs`
3. `Cortexa.Application/Features/ClinicalData/Commands/AddCommands/RecordVitalsCommand.cs`
4. `Cortexa.Infrastructure/Persistence/Repositories/UnitOfWork.cs`
5. `Cortexa.Infrastructure/Persistence/CortexaDbContext.cs`

That path shows almost the whole architecture in one feature.
