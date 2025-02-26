# CRM Solution

A microservices-based Customer Relationship Management system built with .NET 8.

## Project Structure

### Customers Microservice
- `Customers.API` - REST API endpoints
- `Customers.Application` - Application services and business logic
- `Customers.Domain` - Domain entities and interfaces
- `Customers.Infrastructure` - Data access and external services

## Getting Started

1. Clone the repository
2. Navigate to the solution directory
3. Run `dotnet restore`
4. Run `dotnet build`
5. Navigate to src/Customers/Customers.API and run `dotnet run`

## Technologies

- .NET 8
- Entity Framework Core
- SQL Server
- Swagger/OpenAPI 