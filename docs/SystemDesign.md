# Cortexia System Design Diagrams

This document contains Mermaid diagrams that describe Cortexia from a system design perspective.

These diagrams focus on runtime boundaries, dependencies, and infrastructure flow. For entity relationships, use [ERD.md](./ERD.md). For object structure, use [UML.md](./UML.md). For runtime interactions, use [Sequence.md](./Sequence.md).

## System Design

```mermaid
flowchart TB
  Client[Frontend or API Consumer]
  Api[Cortexa.Api]
  App[Cortexa.Application]
  Domain[Cortexa.Domain]
  Infra[Cortexa.Infrastructure]
  Db[(SQL Server)]
  SignalR[SignalR Clients]
  AI[External AI/RAG Service]
  Email[Email Provider]

  Client -->|HTTP REST| Api
  Client <-->|Realtime updates| SignalR
  Api -->|Commands and Queries| App
  App -->|Business rules| Domain
  App -->|Interfaces| Infra
  Infra -->|EF Core| Db
  Infra -->|JWT and Identity storage| Db
  Infra -->|HTTP| AI
  Infra -->|SMTP or provider API| Email
  Api -->|Hub messages| SignalR
```

## 2. Clean Architecture Dependency Direction

```mermaid
flowchart LR
  Api[Cortexa.Api]
  Application[Cortexa.Application]
  Domain[Cortexa.Domain]
  Infrastructure[Cortexa.Infrastructure]
  External[External Services]
  Database[(Database)]

  Api --> Application
  Application --> Domain
  Infrastructure --> Application
  Infrastructure --> Domain
  Api --> Infrastructure
  Infrastructure --> External
  Infrastructure --> Database

  Domain -.no dependency on outer layers.-> Domain
```

## 3. Backend Runtime Request Path

```mermaid
flowchart LR
  Client[Client]
  Middleware[ASP.NET Core Middleware]
  Auth[JWT Authentication and Authorization]
  Controller[API Controller]
  MediatR[MediatR]
  Validation[Validation Behavior]
  Handler[Application Handler]
  UnitOfWork[UnitOfWork and Repositories]
  DbContext[CortexaDbContext]
  Sql[(SQL Server)]

  Client --> Middleware
  Middleware --> Auth
  Auth --> Controller
  Controller --> MediatR
  MediatR --> Validation
  Validation --> Handler
  Handler --> UnitOfWork
  UnitOfWork --> DbContext
  DbContext --> Sql
  Sql --> DbContext
  DbContext --> UnitOfWork
  UnitOfWork --> Handler
  Handler --> Controller
  Controller --> Client
```



## 5. Realtime Monitoring and Alert Design

```mermaid
flowchart LR
  Nurse[Nurse Client]
  Doctor[Doctor Client]
  Api[Cortexia API]
  Handler[Clinical Command Handler]
  NewsCalculator[NEWS Calculator]
  AlertEntity[Alert Entity]
  NotificationService[INotificationService]
  AlertHub["/hubs/alerts"]
  MonitoringHub["/hubs/monitoring"]
  Db[(SQL Server)]

  Nurse -->|Record vitals| Api
  Api --> Handler
  Handler --> NewsCalculator
  NewsCalculator -->|Risk score| Handler
  Handler --> AlertEntity
  Handler --> Db
  Handler --> NotificationService
  NotificationService --> AlertHub
  NotificationService --> MonitoringHub
  AlertHub --> Doctor
  AlertHub --> Nurse
  MonitoringHub --> Doctor
  MonitoringHub --> Nurse
```

## 6. Smart Assistant System Design

```mermaid
flowchart TB
  Doctor[Doctor Client]
  SmartController[SmartAssistantController]
  MediatR[MediatR]
  Handler[Smart Assistant Handler]
  AIInterface[IAIService]
  PythonRAG[PythonRAGService]
  HttpClient[AIHttpClient]
  ExternalRAG[External AI/RAG Service]
  RagRepo[RAG Repository]
  KnowledgeRepo[Knowledge Source Repository]
  Db[(SQL Server)]

  Doctor -->|Ask question or upload source| SmartController
  SmartController --> MediatR
  MediatR --> Handler
  Handler --> AIInterface
  AIInterface --> PythonRAG
  PythonRAG --> HttpClient
  HttpClient --> ExternalRAG
  ExternalRAG --> HttpClient
  HttpClient --> PythonRAG
  PythonRAG --> AIInterface
  AIInterface --> Handler
  Handler --> RagRepo
  Handler --> KnowledgeRepo
  RagRepo --> Db
  KnowledgeRepo --> Db
  Handler --> SmartController
  SmartController --> Doctor
```

## 7. Deployment Dependency View

```mermaid
flowchart TB
  User[User Browser]
  Docs[Documentation Site]
  ApiHost[ASP.NET Core API Host]
  SqlServer[(SQL Server)]
  AiHost[AI/RAG Host]
  Mail[SMTP or Email Service]
  Storage[Cloudinary Storage]

  User --> Docs
  User --> ApiHost
  ApiHost --> SqlServer
  ApiHost --> AiHost
  ApiHost --> Mail
  ApiHost --> Storage

  subgraph ApiConfig[Required API Configuration]
    Conn[DefaultConnection]
    Jwt[JwtSettings]
    Email[EmailSettings]
    AIBase[AIService:BaseUrl]
    Cloudinary[CloudinarySettings]
  end

  ApiHost -.requires.-> Conn
  ApiHost -.requires.-> Jwt
  ApiHost -.requires.-> Email
  ApiHost -.requires.-> AIBase
  ApiHost -.requires.-> Cloudinary
```
