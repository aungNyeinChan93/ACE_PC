# ACE_PC: Project Overview & Architectural Flow

Welcome to the **ACE_PC** project. This document provides a high-level overview of the application's design, system architecture, folder structure, and the end-to-end data/request flows.

---

## 📖 Project Overview
**ACE_PC** is a multi-tier, modern .NET application designed for managing and sharing **Quotes** with social features (likes, comments, and categorizations), as well as a **Todo** system. The application is built using a decoupled architecture consisting of an ASP.NET Core Web API backend, a Blazor Server frontend using MudBlazor, and SQL Server managed via Entity Framework Core.

---

## 🏗️ System Architecture & Project Structure
The solution is organized into five main projects following clean architecture principles:

```
ACE_PC/
├── ACE_PC.Domain/          # Core models, DTOs, and interface definitions
├── ACE_PC.Database/        # EF Core DbContext, migrations, and database configs
├── ACE_PC.BL/              # Business Logic services implementation
├── ACE_PC.Api/             # REST API Controllers, authentication, and routing
└── ACE_PC.BlazorServer/    # Blazor Server UI, UseCases, and custom auth providers
```

### 1. 📦 [ACE_PC.Domain](file:///d:/slh/proj/ACE_PC/ACE_PC.Domain)
This is the core project containing no external dependencies other than domain helper structures.
*   **Entities:** Core data models such as [User](file:///d:/slh/proj/ACE_PC/ACE_PC.Domain/Entity/User.cs), [Role](file:///d:/slh/proj/ACE_PC/ACE_PC.Domain/Entity/Role.cs), [Quote](file:///d:/slh/proj/ACE_PC/ACE_PC.Domain/Entity/Quote.cs), [Category](file:///d:/slh/proj/ACE_PC/ACE_PC.Domain/Entity/Category.cs), [Author](file:///d:/slh/proj/ACE_PC/ACE_PC.Domain/Entity/Author.cs), [Like](file:///d:/slh/proj/ACE_PC/ACE_PC.Domain/Entity/Like.cs), [Comment](file:///d:/slh/proj/ACE_PC/ACE_PC.Domain/Entity/Comment.cs), and [Todo](file:///d:/slh/proj/ACE_PC/ACE_PC.Domain/Entity/Todo.cs).
*   **Interfaces:** Service definitions (e.g., `IQuoteService`, `IUserService`, `IAuthService`) that define the contract for the business logic layer.
*   **DTOs & Models:** Request and response schemas used for transferring data between layers and via API endpoints.

### 2. 🗄️ [ACE_PC.Database](file:///d:/slh/proj/ACE_PC/ACE_PC.Database)
Manages persistence and database schema configuration.
*   Uses Entity Framework Core with SQL Server.
*   Includes [AppDbContext](file:///d:/slh/proj/ACE_PC/ACE_PC.Database/Data/AppDbContext.cs) which configures relationships, such as the composite primary key for `Like` (UserId & QuoteId) and foreign key cascade-delete behaviors.

### 3. ⚙️ [ACE_PC.BL](file:///d:/slh/proj/ACE_PC/ACE_PC.BL) (Business Logic)
Contains the concrete implementations of the services defined in the Domain layer.
*   Implements [UserService](file:///d:/slh/proj/ACE_PC/ACE_PC.BL/Services/UserService.cs), [QuoteService](file:///d:/slh/proj/ACE_PC/ACE_PC.BL/Services/QuoteService.cs), `AuthService`, etc.
*   Processes business validation, password hashing, and coordinates database operations through `AppDbContext`.

### 4. 🌐 [ACE_PC.Api](file:///d:/slh/proj/ACE_PC/ACE_PC.Api)
Exposes RESTful endpoints for consumption by frontend clients.
*   **Controllers:** Maps HTTP verbs (`GET`, `POST`, `PUT`, `DELETE`) to corresponding business logic service calls.
*   **Authentication:** Configured to secure routes using **JWT Bearer Tokens**.
*   **API Docs:** Employs Scalar UI (`Scalar.AspNetCore`) for modern, interactive OpenAPI documentation in development mode.

### 5. 🖥️ [ACE_PC.BlazorServer](file:///d:/slh/proj/ACE_PC/ACE_PC.BlazorServer)
The user interface built on ASP.NET Core Blazor Server.
*   **UI Components:** Designed using the MudBlazor component library.
*   **UseCases:** Front-end orchestration classes (e.g., [QuoteUseCases](file:///d:/slh/proj/ACE_PC/ACE_PC.BlazorServer/UseCases/Quotes/QuoteUseCases.cs)) that manage API communication via `HttpClient`.
*   **State & Authentication:** Custom `AuthenticationStateProvider` (`CustomeAuthenticationProvider`) stores JWT tokens in local storage (`ProtectedLocalStorage`) and handles user session state.

---

## 🔄 End-to-End Request & Data Flow
The sequence diagram below represents how a typical request (e.g., requesting quote data) flows through the application:

```mermaid
sequenceDiagram
    autonumber
    actor User as User/Browser
    participant Blazor as Blazor Server UI
    participant UseCase as UseCase (HttpClient)
    participant API as Web API Controller
    participant Service as Business Service (BL)
    participant DB as AppDbContext (EF Core)
    database SqlServer as SQL Server

    User->>Blazor: Performs Action (e.g., Views Quotes)
    Blazor->>UseCase: Calls QuoteUseCases.GetAllQuotes()
    Note over UseCase: Retrieves JWT token from LocalStorage &<br/>attaches to Authorization Header
    UseCase->>API: HTTP GET /api/quotes
    API->>API: Validates JWT Bearer Token
    API->>Service: Calls IQuoteService.GetAllAsync()
    Service->>Service: Applies Business Rules & Pagination
    Service->>DB: AppDbContext.Quotes.ToList()
    DB->>SqlServer: Executes SELECT SQL Query
    SqlServer-->>DB: Returns Rowset Data
    DB-->>Service: Maps to Entity Models
    Service-->>API: Returns mapped DTOs / ResultModel
    API-->>UseCase: Returns JSON payload
    UseCase-->>Blazor: Returns deserialized ResultModel
    Blazor-->>User: Re-renders page with MudBlazor components
```

---

## 🔑 Key Features
*   **User & Authentication:** JWT token authentication, roles (Admin/User), custom state management in Blazor.
*   **Quotes Feed:** Searchable quotes feed with pagination, filtered by Author and Category.
*   **Social Interactions:** Users can like and comment on quotes (composite key-based likes, restricted and cascade deleting rules on comments).
*   **Category & Author Management:** Dynamic taxonomy setup for organizing content.
