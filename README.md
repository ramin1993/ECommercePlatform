# Microservices architecture ECommercePlatform 

A microservices-based e-commerce platform built to demonstrate authentication, authorization, and inter-service communication using modern technologies and architectural patterns.

## Status
This project is **work in progress**. Core functionalities such as authentication, order creation, and product pricing are implemented, but additional features, optimizations, and tests are still under development.

## Technologies Used
- **Backend**: 
  - C# (.NET 8.0)
  - ASP.NET Core (Web API)
- **Messaging**: 
  - RabbitMQ (via `RabbitMQ.Client`)
- **Serialization**: 
  - Newtonsoft.Json
- **Authentication & Authorization**: 
  - JWT (JSON Web Tokens)
- **API Gateway**: 
  - Ocelot
- **Dependency Injection**: 
  - Built-in ASP.NET Core DI
- **Object Mapping**: 
  - AutoMapper
- **Logging**: 
  - Microsoft.Extensions.Logging
- **Version Control**: 
  - Git (hosted on GitHub)
- **Containerization**: 
  - Docker (via `docker-compose.yaml`, partially implemented)

## Architectural Patterns
- **Microservices**: The application is split into independent services (`ApiGateway`, `ProductService`, `OrderService`, `EventBusService`, etc.) communicating via messaging.
- **Domain-Driven Design (DDD)**: Applied in `OrderService` with entities, repositories, and a service layer.
- **CQRS (partial)**: Separation of read (`GetAllOrdersAsync`) and write (`CreateOrderAsync`) operations in `OrderService`.
- **Event-Driven Architecture**: RabbitMQ is used for asynchronous communication between `OrderService` and `ProductService`.

## Initial Implementation
The project started with the following services and processes:
1. **ApiGateway**:
   - Handles authentication via `/api/auth/login` endpoint using JWT.
   - Routes requests to appropriate microservices using Ocelot.
2. **ProductService**:
   - Manages product-related operations (currently mocked with hardcoded pricing).
   - Responds to price requests from `OrderService` via RabbitMQ.
3. **OrderService**:
   - Allows users to create orders (`CreateOrderAsync`) by requesting product prices from `ProductService` via RabbitMQ.
   - Provides an endpoint for admins to view all orders (`GetAllOrdersAsync`).
   - Implements role-based authorization (`User` for creating, `Admin` for viewing).
4. **EventBusService**:
   - Facilitates messaging between services using RabbitMQ.
   - Implements `Publish` and `Subscribe` methods for price request/response workflows.

## Features Implemented
- **Authentication**: Users and admins log in to obtain JWT tokens.
- **Authorization**: Role-based access control (e.g., only `Admin` can view all orders).
- **Inter-Service Communication**: `OrderService` requests product prices from `ProductService` via RabbitMQ.
- **Basic CRUD**: Order creation and retrieval in `OrderService`.

## Testing
- Unit tests are partially implemented (e.g., `OrderServiceLayer` with Moq for mocking dependencies).
- Integration tests and end-to-end tests are planned but not yet completed.

## Setup Instructions
1. **Prerequisites**:
   - .NET 8.0 SDK
   - RabbitMQ (running locally or via Docker)
   - Git
2. **Clone the Repository**:
   ```bash
   git clone https://github.com/ramin1993/ECommercePlatform.git
   cd ECommercePlatform
