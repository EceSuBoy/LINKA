# LINKA — Microservices E-Commerce Platform

<p align="center">
  A full-stack e-commerce platform built with <strong>ASP.NET Core 8</strong> and a <strong>microservices architecture</strong>, featuring centralized identity, API Gateway routing, polyglot persistence, real-time communication, cloud image storage, shopping cart, order management, discounts, comments, cargo operations, administration tools, and an integrated shopping assistant.
</p>

<p align="center">
  <img src="https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet" alt=".NET 8" />
  <img src="https://img.shields.io/badge/ASP.NET%20Core-MVC%20%26%20Web%20API-512BD4?logo=dotnet" alt="ASP.NET Core" />
  <img src="https://img.shields.io/badge/Architecture-Microservices-blue" alt="Microservices" />
  <img src="https://img.shields.io/badge/API%20Gateway-Ocelot-success" alt="Ocelot" />
  <img src="https://img.shields.io/badge/MongoDB-Catalog-47A248?logo=mongodb&logoColor=white" alt="MongoDB" />
  <img src="https://img.shields.io/badge/PostgreSQL-Messaging-4169E1?logo=postgresql&logoColor=white" alt="PostgreSQL" />
  <img src="https://img.shields.io/badge/Redis-Basket-DC382D?logo=redis&logoColor=white" alt="Redis" />
  <img src="https://img.shields.io/badge/RabbitMQ-Message%20Queue-FF6600?logo=rabbitmq&logoColor=white" alt="RabbitMQ" />
  <img src="https://img.shields.io/badge/Docker-Local%20Infrastructure-2496ED?logo=docker&logoColor=white" alt="Docker" />
</p>

<p align="center">
  <a href="./docs/images/linka-home-assistant.jpg">
    <img src="./docs/images/linka-home-assistant.jpg" alt="LINKA e-commerce storefront with integrated shopping assistant" width="760" />
  </a>
</p>

<p align="center">
  <em>LINKA storefront with dynamic categories, featured products, campaigns, stock information, ratings, and the integrated LINKA Shopping Assistant.</em>
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

**LINKA** is a modular e-commerce application designed around a microservices architecture. Business capabilities are separated into independent services such as Catalog, Basket, Discount, Order, Cargo, Comment, Message, Images, Payment, and Identity rather than being maintained inside a single monolithic application.

The platform combines **Ocelot API Gateway**, **IdentityServer**, **JWT authentication**, **MongoDB**, **Redis**, **SQL Server**, **PostgreSQL**, **Dapper**, **Entity Framework Core**, **RabbitMQ**, **SignalR**, **Google Cloud Storage**, and external API integrations. The Order service additionally applies **Onion Architecture, CQRS, MediatR, and Repository Pattern**.

---

## Key Features

### Customer Experience

- User registration and login with centralized authentication
- Product/category browsing, searching, filtering, and pagination
- Product details, images, ratings, reviews, and stock information
- Featured products, discounts, campaigns, and coupon support
- Redis-backed shopping basket with quantity management
- Checkout and simulated card payment flow
- Automatic stock reduction after successful checkout
- Unique order number generation and order history
- Cargo/customer information and order status tracking
- User profile and wishlist areas
- Product-based buyer-to-buyer messaging
- Integrated shopping assistant for products, categories, and deals
- Weather information through RapidAPI

### Administration

- Admin dashboard with catalog and operational statistics
- Category, product, product detail, image, and brand management
- Slider, feature, special offer, discount, and coupon management
- Comment, user, and role management
- Order management and order status updates
- Cargo company management
- Search, filtering, pagination, and featured-product controls

### Platform / Backend

- Independent REST APIs with Swagger/OpenAPI support
- Ocelot API Gateway and protected service routing
- IdentityServer-based authentication and authorization
- JWT Bearer validation and scope-based permissions
- Polyglot persistence across MongoDB, Redis, SQL Server, and PostgreSQL
- Dapper and Entity Framework Core data-access approaches
- SignalR real-time communication infrastructure
- RabbitMQ producer/consumer messaging
- Google Cloud Storage upload, delete, and signed URL operations
- AJAX / Fetch-based asynchronous UI operations

---

## Architecture

LINKA uses an API Gateway-centered microservices architecture. The ASP.NET Core MVC client communicates with backend services through **Ocelot**, while **IdentityServer** handles authentication and token generation. Each microservice owns a specific responsibility and uses the storage technology appropriate for its workload.

<p align="center">
  <a href="./docs/images/linka-microservice-diagram.png">
    <img src="./docs/images/linka-microservice-diagram.png" alt="LINKA Microservice Architecture Diagram" width="850" />
  </a>
</p>

<p align="center">
  <em>LINKA microservice architecture showing the WebUI client, Ocelot API Gateway, IdentityServer, independent services, and their respective data stores.</em>
</p>

---

## Microservices

| Service / Project | Responsibility | Main Technology / Storage |
|---|---|---|
| **Linka.Catalog** | Products, categories, brands, product details/images, features, offers, sliders, content, and catalog statistics | MongoDB, AutoMapper |
| **Linka.Basket** | User shopping basket operations | Redis |
| **Linka.Discount** | Discount coupons and discount statistics | Dapper, SQL Server |
| **Linka.Order** | Order addresses/details, creation, status, and queries | Onion Architecture, CQRS, MediatR, Repository Pattern, EF Core, SQL Server |
| **Linka.Cargo** | Cargo companies, customers, details, and operations | Repository Pattern, EF Core, SQL Server |
| **Linka.Comment** | Product reviews/comments and comment statistics | EF Core, SQL Server |
| **Linka.Message** | Product-based conversations, messages, read state, and message statistics | EF Core, PostgreSQL |
| **Linka.Images** | Cloud image upload, delete, and signed URL generation | Google Cloud Storage |
| **Linka.Payment** | Payment-service boundary; simulated checkout/payment flow | ASP.NET Core Web API |
| **Linka.IdentityServer** | Authentication, users, roles, clients, API resources, and scopes | IdentityServer4, ASP.NET Core Identity, SQL Server |
| **Linka.OcelotGateway** | Central API routing and protected service access | Ocelot, JWT Bearer |
| **Linka.SignalRRealTimeApi** | Real-time hub and live notification/statistics infrastructure | SignalR |
| **Linka.RabbitMQMessageApi** | Queue-based producer/consumer messaging | RabbitMQ |
| **Linka.RapidApiUI** | External API consumption | RapidAPI, HttpClient, Newtonsoft.Json |
| **Linka.WebUI** | Customer/admin MVC application and API consumption layer | ASP.NET Core MVC, Razor, HttpClient, JavaScript |
| **Linka.DtoLayer** | Shared frontend DTO models | .NET 8 |

---

## Technology Stack

### Backend

- C# / .NET 8
- ASP.NET Core MVC & Web API
- Entity Framework Core
- Dapper
- AutoMapper
- MediatR
- IdentityServer4 / ASP.NET Core Identity
- JWT / JWT Bearer Authentication
- Ocelot API Gateway
- SignalR
- RabbitMQ
- Swagger / OpenAPI
- HttpClient / API Consumption

### Databases & Storage

- MongoDB
- Redis
- Microsoft SQL Server
- PostgreSQL
- SQLite package/support in the IdentityServer project
- Google Cloud Storage

### Frontend / UI

- Razor Views
- HTML5 / CSS3
- JavaScript
- AJAX / Fetch API
- Bootstrap-based UI components
- SignalR JavaScript Client

### Development & Integration Tools

- Docker Desktop
- Portainer
- Postman
- RapidAPI
- Git / GitHub

> **Docker usage:** Docker Desktop is used for LINKA's local infrastructure layer, including multiple SQL Server database containers and Redis, with Portainer used for container management. The .NET application services are currently launched from the solution rather than orchestrated as one complete Docker Compose application.

---

## Application Preview

The main storefront is shown at the top of this README. Additional screens from the shopping, messaging, administration, order, and development workflows are available below. Click an image to open the full-resolution version.

<details>
<summary><strong>View additional application screenshots</strong></summary>
<br>

<table>
  <tr>
    <td width="50%" align="center">
      <strong>Product Listing & Filtering</strong><br><br>
      <a href="./docs/images/product-list.jpg"><img src="./docs/images/product-list.jpg" alt="LINKA product listing and filtering" width="100%"></a>
    </td>
    <td width="50%" align="center">
      <strong>Product Details & Reviews</strong><br><br>
      <a href="./docs/images/product-details-reviews.jpg"><img src="./docs/images/product-details-reviews.jpg" alt="LINKA product details and reviews" width="100%"></a>
    </td>
  </tr>
  <tr>
    <td width="50%" align="center">
      <strong>Redis-Backed Shopping Cart</strong><br><br>
      <a href="./docs/images/shopping-cart.jpg"><img src="./docs/images/shopping-cart.jpg" alt="LINKA shopping cart" width="100%"></a>
    </td>
    <td width="50%" align="center">
      <strong>Simulated Secure Payment</strong><br><br>
      <a href="./docs/images/secure-payment.jpg"><img src="./docs/images/secure-payment.jpg" alt="LINKA secure payment screen" width="100%"></a>
    </td>
  </tr>
  <tr>
    <td width="50%" align="center">
      <strong>Order Completion</strong><br><br>
      <a href="./docs/images/order-completed.jpg"><img src="./docs/images/order-completed.jpg" alt="LINKA order completed screen" width="100%"></a>
    </td>
    <td width="50%" align="center">
      <strong>User Order Details</strong><br><br>
      <a href="./docs/images/user-order-details.jpg"><img src="./docs/images/user-order-details.jpg" alt="LINKA user order details" width="100%"></a>
    </td>
  </tr>
  <tr>
    <td width="50%" align="center">
      <strong>Product-Based User Messaging</strong><br><br>
      <a href="./docs/images/user-to-user-messaging.jpg"><img src="./docs/images/user-to-user-messaging.jpg" alt="LINKA user-to-user messaging" width="100%"></a>
    </td>
    <td width="50%" align="center">
      <strong>Admin Dashboard</strong><br><br>
      <a href="./docs/images/admin-dashboard.jpg"><img src="./docs/images/admin-dashboard.jpg" alt="LINKA admin dashboard" width="100%"></a>
    </td>
  </tr>
  <tr>
    <td colspan="2" align="center">
      <strong>Admin Product Management</strong><br><br>
      <a href="./docs/images/admin-product-management.jpg"><img src="./docs/images/admin-product-management.jpg" alt="LINKA admin product management" width="90%"></a>
    </td>
  </tr>
  <tr>
    <td colspan="2" align="center">
      <strong>Docker Local Infrastructure</strong><br><br>
      <a href="./docs/images/docker-local-infrastructure.png"><img src="./docs/images/docker-local-infrastructure.png" alt="LINKA local Docker infrastructure" width="95%"></a>
    </td>
  </tr>
</table>

</details>
