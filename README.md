# ECommercePlatform

Welcome to **ECommercePlatform**, a microservices-based e-commerce solution designed to showcase modern software development practices. This project is built as a portfolio piece to demonstrate expertise in microservices architecture, authentication, and event-driven communication.

## Overview
This repository serves as the foundation for an e-commerce platform split into multiple independent services. The `main` branch represents the stable baseline, while feature branches (e.g., `feature/auth-integration`) contain ongoing development and specific implementations.

## Key Features
- **Microservices Architecture**: Separate services for API Gateway, Product, Order, and Event Bus.
- **Scalable Design**: Built with extensibility in mind for adding new services (e.g., User, Payment).
- **Technology Stack**: Utilizes .NET 8.0, ASP.NET Core, RabbitMQ, JWT, and more.

## Technologies Used
- **Backend**: C# (.NET 8.0), ASP.NET Core
- **Messaging**: RabbitMQ
- **Authentication**: JWT
- **API Gateway**: Ocelot
- **Tools**: Git, Docker (planned)
- **Database**: Sql,PostgreSql

## Architectural Patterns
- **Microservices**: Independent, loosely coupled services.
- **Event-Driven**: Asynchronous communication between services.
- **Domain-Driven Design**: Applied in core business logic.

## Current Progress
- The `main` branch provides the initial structure and setup for the microservices.
- Detailed implementations (e.g., authentication, order processing) are available in feature branches like `feature/auth-integration`.

## Getting Started
1. **Clone the Repository**:
   ```bash
   git clone https://github.com/ramin1993/ECommercePlatform.git
   cd ECommercePlatform
