# TelecomWeb API

TelecomWeb functions as a RESTful API built with C# and ASP.NET Core 8.0. The project serves as a comprehensive simulation of a modern telecommunications billing and client management system. You will find enterprise-level scalability built right in. The project solves common telecom challenges, including tiered user management and dynamic billing.

The primary goal involves demonstrating advanced object-oriented programming (OOP) paradigms and strict adherence to SOLID principles. By employing a clean architecture approach, you keep business logic, data access, and API routing completely separated.

## Architecture & Core Design Patterns

The architecture relies heavily on established GoF (Gang of Four) design patterns. You will see the system grow without requiring massive rewrites of existing code.

### SOLID Principles in Action:
*   **Single Responsibility Principle (SRP):** Controllers strictly handle HTTP requests and routing. Dedicated service classes (BillingService, ClientService) manage core business logic.
*   **Open/Closed Principle (OCP):** You easily extend the system without modifying existing code. Add new client tiers or billing rules by adding new classes implementing existing interfaces.
*   **Dependency Injection (DI):** You inject services, database interfaces (IClientDatabase), and billing strategies into the application pipeline via Program.cs. This loose coupling simplifies lifecycle management. You create a highly testable environment.
*   **Factory Method Pattern:** ClientFactory manages complex instantiation logic for various client types. The factory evaluates database records and dynamically constructs either ProClient or LiteClient.
*   **Strategy Pattern:** Pricing and billing calculations operate dynamically. By pairing IPriceStrategy with concrete implementations like ProPriceStrategy and LitePriceStrategy, you calculate costs at runtime based on user tiers.
*   **Proxy Pattern:** DatabaseProxy controls and optimizes access to the underlying database. This layer acts as a gatekeeper for caching mechanisms or query logging. You ensure MySqlClientDatabase never gets overloaded directly.

## Comprehensive Feature Set

### Advanced Client Management
*   Seamlessly onboard, retrieve, update, and remove clients.
*   The domain model inherently supports diverse user tiers. You differentiate between basic users (LiteClient) and power users (ProClient with customizable internet speeds and data caps).

### Service Provisioning & Add-ons
*   Dynamically assign or revoke telecommunication services (e.g., static IPs, international calling, IPTV) to specific client profiles.
*   Maintain an accurate ledger of active services.

### Secure & Reliable Data Access
*   The system uses pure ADO.NET with MySqlConnector for high-performance database interactions.
*   The repository layer heavily employs ACID-compliant database transactions. You prevent partial state modifications during complex operations like creating a client and their security profile simultaneously.

## Technologies Used
*   **C# / .NET 8.0:** The core framework powering the API.
*   **MySqlConnector:** A high-performance, async-first MySQL driver for .NET.
*   **Swagger (OpenAPI):** Provides automatic API documentation and a built-in testing interface.

## Getting Started

### Prerequisites
*   [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
*   A running MySQL database instance.

### Configuration

To run this project locally, you must configure your database connection securely using .NET User Secrets. You must not hardcode credentials.

1.  Navigate to the project directory containing TelecomWeb.csproj.
2.  Initialize user secrets:
```bash
    dotnet user-secrets init
    ```
3.  Set your connection string:
```bash
    dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=YOUR_SERVER; Port=3306; Database=TelecomDb; User=YOUR_USER; Password=YOUR_PASSWORD; SslMode=Required;"
    ```

### Running the Application

1.  Restore dependencies:
```bash
    dotnet restore
    ```
2.  Run the API:
```bash
    dotnet run
    ```
3.  Navigate to `https://localhost:<port>/swagger` in your browser to interact with the API endpoints.

## Testing

The project includes a comprehensive suite of unit tests. You execute them using the .NET CLI:

```bash
dotnet test
