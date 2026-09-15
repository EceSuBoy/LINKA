[README (2).md](https://github.com/user-attachments/files/32251906/README.2.md)
# LINKA — Microservices E-Commerce Platform

<p align="center">
  A full-stack e-commerce platform built with <strong>ASP.NET Core 8</strong> and a <strong>microservices architecture</strong>, featuring centralized identity, API Gateway routing, polyglot persistence, real-time communication, cloud image storage, shopping cart, order management, discounts, comments, cargo operations, admin tools, and a built-in shopping assistant.
</p>

<p align="center">
  <img src="https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet" alt=".NET 8" />
  <img src="https://img.shields.io/badge/ASP.NET%20Core-MVC%20%26%20Web%20API-512BD4?logo=dotnet" alt="ASP.NET Core" />
  <img src="https://img.shields.io/badge/Architecture-Microservices-blue" alt="Microservices" />
  <img src="https://img.shields.io/badge/API%20Gateway-Ocelot-success" alt="Ocelot" />
  <img src="https://img.shields.io/badge/MongoDB-Catalog-47A248?logo=mongodb&logoColor=white" alt="MongoDB" />
  <img src="https://img.shields.io/badge/PostgreSQL-Messaging-4169E1?logo=postgresql&logoColor=white" alt="PostgreSQL" />
  <img src="https://img.shields.io/badge/Redis-Basket-DC382D?logo=redis&logoColor=white" alt="Redis" />
  <img src="https://img.shields.io/badge/RabbitMQ-Messaging-FF6600?logo=rabbitmq&logoColor=white" alt="RabbitMQ" />
</p>

<p align="center">
  <a href="docs/images/linka-home-assistant.jpg">
    <img src="docs/images/linka-home-assistant.jpg" alt="LINKA e-commerce storefront with integrated shopping assistant" width="100%" />
  </a>
</p>

<p align="center">
  <em>LINKA storefront with dynamic categories, featured products, campaigns, stock information, ratings and the integrated LINKA Shopping Assistant.</em>
</p>

---

## Table of Contents

- [About the Project](#about-the-project)
- [Key Features](#key-features)
- [Architecture](#architecture)
- [Microservices](#microservices)
- [Technology Stack](#technology-stack)
- [Application Preview](#application-preview)

---

## About the Project

**LINKA** is a modular e-commerce application designed around a microservices architecture. Instead of storing all responsibilities in a single monolithic application, business capabilities are separated into independent services such as Catalog, Basket, Discount, Order, Cargo, Comment, Message, Images, Payment, and Identity.

The project demonstrates practical use of modern backend concepts including:

- Service-oriented / microservices architecture
- API Gateway routing with Ocelot
- Centralized authentication and authorization
- JWT access tokens and JWT Bearer authentication
- Polyglot persistence
- CQRS and MediatR
- Repository Pattern
- Onion Architecture
- RESTful API design
- API consumption from an ASP.NET Core MVC frontend
- Redis-based basket storage
- MongoDB-based catalog management
- PostgreSQL-based messaging
- SQL Server-based transactional services
- Dapper-based data access
- RabbitMQ messaging
- SignalR real-time communication
- Google Cloud Storage integration for images
- RapidAPI consumption
- Swagger/OpenAPI documentation
- Postman-based API testing
- AJAX / Fetch-based asynchronous UI operations

---

## Key Features

### Customer Experience

- User registration and login
- Secure authenticated sessions through IdentityServer
- Product and category browsing
- Product detail pages
- Product images and cloud-hosted image URLs
- Search and paginated product listing
- Category-based product filtering
- Price and product listing controls
- Featured products
- Discounted products and campaign support
- Stock tracking and stock validation
- Redis-backed shopping basket
- Discount coupon support
- Checkout flow
- Test-card validation for simulated payment
- Automatic stock reduction after successful checkout
- Order creation with order details
- Unique order number generation
- User order history
- Cargo/customer information
- Product comments and comment statistics
- User profile area
- Buyer-to-buyer conversation/message infrastructure
- Built-in shopping assistant for products, categories, and deals
- Weather information through RapidAPI

### Administration

- Admin dashboard and admin area
- Category management
- Product management
- Product detail management
- Product image management
- Brand management
- Feature and slider management
- Special offer management
- Offer discount management
- Coupon management
- Comment management
- User management
- Role management
- Order management
- Order status updates
- Cargo management
- Statistics and reporting endpoints

### Platform / Backend

- Independent REST APIs
- Ocelot API Gateway
- IdentityServer-based centralized security
- Scope-based service permissions
- JWT Bearer validation in protected services
- Multiple databases selected according to service requirements
- Swagger support across Web APIs
- SignalR hub for real-time updates
- RabbitMQ producer/consumer sample service
- Google Cloud Storage file upload, delete, and signed URL operations

---

## Architecture

LINKA uses an API Gateway-centered microservices architecture. The ASP.NET Core MVC client communicates with backend services through **Ocelot**, while **IdentityServer** handles authentication and token generation. Each microservice owns its specific responsibility and uses the storage technology best suited to its workload.

<p align="center">
  <a href="docs/images/linka-microservice-diagram.png">
    <img src="docs/images/linka-microservice-diagram.png" alt="LINKA Microservice Architecture Diagram" width="100%" />
  </a>
</p>

<p align="center">
  <em>LINKA microservice architecture showing the WebUI client, Ocelot API Gateway, IdentityServer, independent services and their respective data stores.</em>
</p>

---

## Microservices

| Service / Project | Responsibility | Main Technology / Storage |
|---|---|---|
| **Linka.Catalog** | Products, categories, brands, product details, product images, features, offers, sliders, contacts, about content, catalog statistics | MongoDB, AutoMapper |
| **Linka.Basket** | User shopping basket operations | Redis |
| **Linka.Discount** | Discount coupons and discount statistics | Dapper, SQL Server |
| **Linka.Order** | Order addresses, order details, order creation, order status and order queries | Onion Architecture, CQRS, MediatR, Repository Pattern, EF Core, SQL Server |
| **Linka.Cargo** | Cargo companies, customers, details and operations | Layered architecture, Repository Pattern, EF Core, SQL Server |
| **Linka.Comment** | Product reviews/comments and comment statistics | EF Core, SQL Server |
| **Linka.Message** | Conversations, inbox/sent messages, conversation messages, read-state operations and message statistics | EF Core, PostgreSQL |
| **Linka.Images** | Cloud image upload, delete and signed URL generation | Google Cloud Storage |
| **Linka.Payment** | Payment-service boundary used by the gateway; checkout/payment simulation is handled in the WebUI flow | ASP.NET Core Web API |
| **Linka.IdentityServer** | Authentication, users, roles, clients, API resources and scopes | IdentityServer4, ASP.NET Core Identity, SQL Server |
| **Linka.OcelotGateway** | Central API routing and protected service access | Ocelot, JWT Bearer |
| **Linka.SignalRRealTimeApi** | Real-time hub and live statistics/notification infrastructure | SignalR |
| **Linka.RabbitMQMessageApi** | Queue-based messaging example / producer-consumer operations | RabbitMQ |
| **Linka.RapidApiUI** | External API consumption example | RapidAPI, HttpClient, Newtonsoft.Json |
| **Linka.WebUI** | Main customer/admin MVC application and API consumption layer | ASP.NET Core MVC, HttpClient, Razor, JavaScript |
| **Linka.DtoLayer** | Shared frontend DTO models | .NET 8 |

---

## Technology Stack

### Backend

- C#
- .NET 8
- ASP.NET Core 8
- ASP.NET Core MVC
- ASP.NET Core Web API
- Entity Framework Core
- Dapper
- AutoMapper
- MediatR
- IdentityServer4
- ASP.NET Core Identity
- JWT / JSON Web Token
- JWT Bearer Authentication
- Ocelot API Gateway
- SignalR
- RabbitMQ
- Swagger / OpenAPI
- HttpClient / API Consume

### Databases and Storage

- MongoDB
- Redis
- Microsoft SQL Server
- PostgreSQL
- SQLite package/support within the IdentityServer project
- Google Cloud Storage

### Frontend / UI

- Razor Views
- HTML5
- CSS3
- JavaScript
- AJAX
- Fetch API
- Bootstrap-based UI components
- SignalR JavaScript Client

### API / Development Tools

- Swagger
- Postman
- RapidAPI
- Git / GitHub
- Docker for local infrastructure/dependency workflows

> **Docker note:** the current repository snapshot does not contain a `Dockerfile` or `docker-compose.yml`. Docker is used as part of the development/infrastructure workflow, but container definitions should be added to the repository if fully reproducible containerized startup is desired.

---

## Application Preview

The main storefront is shown at the top of this README. Additional screens from the complete shopping, messaging, order and administration flows are available below.

<details>
<summary><strong>View application screenshots</strong></summary>
<br>

<table>
  <tr>
    <td width="50%" align="center"><strong>Product Listing & Filtering</strong><br><img src="docs/images/product-list.jpg" alt="LINKA product listing and filtering" width="100%"></td>
    <td width="50%" align="center"><strong>Product Details & Reviews</strong><br><img src="docs/images/product-details-reviews.jpg" alt="LINKA product details and reviews" width="100%"></td>
  </tr>
  <tr>
    <td width="50%" align="center"><strong>Redis-Backed Shopping Cart</strong><br><img src="docs/images/shopping-cart.jpg" alt="LINKA shopping cart" width="100%"></td>
    <td width="50%" align="center"><strong>Simulated Secure Payment</strong><br><img src="docs/images/secure-payment.jpg" alt="LINKA secure payment screen" width="100%"></td>
  </tr>
  <tr>
    <td width="50%" align="center"><strong>Order Completion</strong><br><img src="docs/images/order-completed.jpg" alt="LINKA order completed screen" width="100%"></td>
    <td width="50%" align="center"><strong>User Order Details</strong><br><img src="docs/images/user-order-details.jpg" alt="LINKA user order details" width="100%"></td>
  </tr>
  <tr>
    <td width="50%" align="center"><strong>Product-Based User Messaging</strong><br><img src="docs/images/user-to-user-messaging.jpg" alt="LINKA user-to-user messaging" width="100%"></td>
    <td width="50%" align="center"><strong>Admin Dashboard</strong><br><img src="docs/images/admin-dashboard.jpg" alt="LINKA admin dashboard" width="100%"></td>
  </tr>
  <tr>
    <td colspan="2" align="center"><strong>Admin Product Management</strong><br><img src="docs/images/admin-product-management.jpg" alt="LINKA admin product management" width="85%"></td>
  </tr>
</table>

</details>
