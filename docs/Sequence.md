# Cortexia Sequence Diagrams

This document contains Mermaid sequence diagrams for important Cortexia runtime flows.

Source of truth:

- `Cortexa.Api/Program.cs`
- `Cortexa.Api/Controllers`
- `Cortexa.Application/Features`
- `Cortexa.Infrastructure/DependencyInjection.cs`
- `Cortexa.Infrastructure/Persistence/CortexaDbContext.cs`
- `Cortexa.Infrastructure/External`

## 1. Login and JWT Issuing

```mermaid
sequenceDiagram
  actor User
  participant Client
  participant AuthController
  participant MediatR
  participant LoginHandler
  participant IdentityService
  participant UserManager
  participant JwtTokenGenerator

  User->>Client: Enter email and password
  Client->>AuthController: POST /api/Auth/login
  AuthController->>MediatR: Send LoginCommand
  MediatR->>LoginHandler: Dispatch command
  LoginHandler->>IdentityService: Validate credentials
  IdentityService->>UserManager: Find user and check password
  UserManager-->>IdentityService: User validation result

  alt Valid credentials
    IdentityService->>JwtTokenGenerator: Generate JWT
    JwtTokenGenerator-->>IdentityService: Token
    IdentityService-->>LoginHandler: Auth result
    LoginHandler-->>AuthController: AuthResponseDto
    AuthController-->>Client: 200 OK + JWT
  else Invalid credentials
    IdentityService-->>LoginHandler: Failure
    LoginHandler-->>AuthController: Unauthorized result
    AuthController-->>Client: 401 Unauthorized
  end
```

## 2. Authenticated API Request

```mermaid
sequenceDiagram
  actor User
  participant Client
  participant ExceptionMiddleware
  participant AuthMiddleware as Authentication/Authorization
  participant Controller
  participant MediatR
  participant ValidationBehavior
  participant Handler
  participant Repository
  participant DbContext
  participant SqlServer

  User->>Client: Perform protected action
  Client->>ExceptionMiddleware: HTTP request with Bearer token
  ExceptionMiddleware->>AuthMiddleware: Continue request
  AuthMiddleware->>AuthMiddleware: Validate JWT and roles

  alt Authorized
    AuthMiddleware->>Controller: Route to action
    Controller->>MediatR: Send command/query
    MediatR->>ValidationBehavior: Run validators
    ValidationBehavior->>Handler: Continue if valid
    Handler->>Repository: Execute persistence operation
    Repository->>DbContext: Query or track changes
    DbContext->>SqlServer: SQL command
    SqlServer-->>DbContext: Result
    DbContext-->>Repository: Entity/data
    Repository-->>Handler: Result
    Handler-->>Controller: DTO/result
    Controller-->>Client: HTTP response
  else Unauthorized or forbidden
    AuthMiddleware-->>Client: 401 Unauthorized or 403 Forbidden
  end

  alt Exception thrown
    Controller-->>ExceptionMiddleware: Exception bubbles up
    ExceptionMiddleware-->>Client: Standard error response
  end
```

## 3. Record Vital Signs and Trigger Alert

```mermaid
sequenceDiagram
  actor Nurse
  participant Client
  participant VitalSignsController
  participant MediatR
  participant RecordVitalsHandler
  participant NewsCalculator
  participant UnitOfWork
  participant NotificationService
  participant AlertHub
  participant DbContext
  participant SqlServer

  Nurse->>Client: Enter vital signs
  Client->>VitalSignsController: POST /api/admissions/{admissionId}/vitals
  VitalSignsController->>MediatR: Send RecordVitalsCommand
  MediatR->>RecordVitalsHandler: Dispatch command
  RecordVitalsHandler->>NewsCalculator: Calculate NEWS score
  NewsCalculator-->>RecordVitalsHandler: Score and risk level
  RecordVitalsHandler->>UnitOfWork: Add VitalSigns

  alt Medium or high risk
    RecordVitalsHandler->>UnitOfWork: Add Alert
    RecordVitalsHandler->>NotificationService: Send alert notification
    NotificationService->>AlertHub: Publish realtime alert
    AlertHub-->>Client: Alert update
  end

  RecordVitalsHandler->>UnitOfWork: SaveChangesAsync
  UnitOfWork->>DbContext: SaveChangesAsync
  DbContext->>DbContext: Apply metadata, soft delete, audit logs
  DbContext->>SqlServer: Persist changes
  SqlServer-->>DbContext: Save result
  DbContext-->>UnitOfWork: Rows affected
  UnitOfWork-->>RecordVitalsHandler: Save completed
  RecordVitalsHandler-->>VitalSignsController: New record id/result
  VitalSignsController-->>Client: 201 Created
```

## 4. Smart Assistant RAG Question

```mermaid
sequenceDiagram
  actor Doctor
  participant Client
  participant SmartAssistantController
  participant MediatR
  participant AskRAGHandler
  participant AIService as IAIService
  participant PythonRAGService
  participant AIHttpClient
  participant ExternalRAG as External AI/RAG Service
  participant RagRepository
  participant DbContext

  Doctor->>Client: Ask clinical question
  Client->>SmartAssistantController: POST /api/SmartAssistant/rag/ask
  SmartAssistantController->>MediatR: Send AskRAGQueryCommand
  MediatR->>AskRAGHandler: Dispatch command
  AskRAGHandler->>AIService: Ask question with context
  AIService->>PythonRAGService: Execute RAG request
  PythonRAGService->>AIHttpClient: POST to external AI service
  AIHttpClient->>ExternalRAG: Request answer
  ExternalRAG-->>AIHttpClient: Generated answer and metadata
  AIHttpClient-->>PythonRAGService: AI response
  PythonRAGService-->>AIService: Parsed response
  AIService-->>AskRAGHandler: RAG answer
  AskRAGHandler->>RagRepository: Store query and response
  RagRepository->>DbContext: Add RAGQuery
  DbContext-->>RagRepository: Track entity
  AskRAGHandler-->>SmartAssistantController: RAGResponseDto
  SmartAssistantController-->>Client: 200 OK + answer
```

## 5. EF Core Save, Soft Delete, and Audit

```mermaid
sequenceDiagram
  participant Handler
  participant UnitOfWork
  participant DbContext as CortexaDbContext
  participant ChangeTracker
  participant CurrentUserService
  participant DateTimeService
  participant AuditLogs
  participant SqlServer

  Handler->>UnitOfWork: SaveChangesAsync
  UnitOfWork->>DbContext: SaveChangesAsync
  DbContext->>DateTimeService: Get current time
  DateTimeService-->>DbContext: Current time
  DbContext->>CurrentUserService: Get current user id
  CurrentUserService-->>DbContext: User id
  DbContext->>ChangeTracker: Find changed BaseEntity entries
  ChangeTracker-->>DbContext: Added, modified, deleted entries
  DbContext->>DbContext: Convert deletes to soft deletes
  DbContext->>DbContext: Apply created/modified/deleted metadata
  DbContext->>DbContext: Build audit entries for IAuditableEntity
  DbContext->>AuditLogs: Add generated audit rows
  DbContext->>SqlServer: Save all changes in one EF save
  SqlServer-->>DbContext: Rows affected
  DbContext-->>UnitOfWork: Save result
  UnitOfWork-->>Handler: Save completed
```
