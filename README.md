# WebApiPlatform

A modular Web API solution built with ASP.NET Core, designed to separate responsibilities and enable scalable, maintainable development.

## 🧩 Project Structure

This repository contains multiple solutions, each responsible for a specific part of the platform:

### 📁 Ecommerce_SharedLiberarySolution
Contains shared code used across other services, including:
- DTOs (Data Transfer Objects)
- Custom exceptions
- Common services/utilities

> Helps maintain consistency and reuse logic across APIs.

---

### 📁 ECommerce.Authentication.Solution
Handles authentication and authorization for the platform using:
- JWT (JSON Web Tokens)
- User login & registration
- Token validation

> Centralizes identity and access control logic.

---

### 📁 Ecommerce.OrderApi.Solution
API responsible for managing orders:
- Create, update, and track customer orders
- Link orders with products and users

> Designed for handling all order-related business logic.

---

### 📁 Ecommerce_ProductAPISolution
Responsible for managing products in the system:
- CRUD operations on products
- Product categorization
- Basic validation and logic

> Acts as the core inventory service of the platform.

---

### 📁 Ecommerce.ApiGateway.Solution
A gateway layer that routes requests to the appropriate services using **Ocelot**:
- Routes traffic to Auth, Order, and Product APIs
- Provides a single entry point for external clients
- Handles basic request forwarding, security, and logging

> Simplifies communication and abstracts internal API structure.

---

## ⚙️ Technologies Used

- ASP.NET Core
- Entity Framework Core
- Ocelot API Gateway
- JWT Authentication
- Clean Architecture principles
- SQL Server

---

## 🚀 Getting Started

1. Clone this repository:
   ```bash
   git clone https://github.com/your-username/WebApiPlatform.git
