# Firmeza.API

A comprehensive REST API built with C# and ASP.NET Core for managing products and sales operations. This application provides robust authentication, product management, and sales tracking functionalities.

## Table of Contents

- [Project Overview](#project-overview)
- [Features](#features)
- [Architecture](#architecture)
- [Class Diagram](#class-diagram)
- [Database Schema (DER)](#database-schema-der)
- [Installation](#installation)
- [Configuration](#configuration)
- [Usage](#usage)
- [API Endpoints](#api-endpoints)
- [Technologies](#technologies)

## Project Overview

Firmeza.API is designed to provide a scalable and secure backend solution for managing:
- User authentication and authorization using JWT
- Product catalog management
- Sales transactions and tracking
- Sales-Product relationships

The application follows Clean Architecture principles with proper separation of concerns through Controllers, Services, Repositories, and Data Access layers.

## Features

✅ **JWT Authentication** - Secure user authentication with token-based access  
✅ **Product Management** - Create, read, update, and delete products  
✅ **Sales Management** - Track and manage sales transactions  
✅ **Role-Based Access Control** - Implement different permission levels  
✅ **Email Notifications** - SMTP configuration for transactional emails  
✅ **Comprehensive Error Handling** - Standardized API responses  
✅ **Database Migrations** - Entity Framework Core support  
✅ **Docker Support** - Containerized deployment ready  

## Architecture

The project follows a layered architecture pattern:

```
Firmeza.API/
├── Controllers/           # HTTP request handlers
├── Services/             # Business logic layer
├── Repositories/         # Data access abstraction
├── Data/
│   ├── AppDbContext.cs   # Entity Framework DbContext
│   ├── Entities/         # Domain entities
│   └── Seeders/          # Database seed data
├── DTOs/                 # Data Transfer Objects
├── Interfaces/           # Abstraction contracts
├── Configs/              # Configuration classes
├── Mappings/             # AutoMapper profiles
└── Responses/            # Standardized response models
```

## Class Diagram

```
┌─────────────────────────────────────────────────────────────────────┐
│                          User (Entity)                              │
├─────────────────────────────────────────────────────────────────────┤
│ - UserId: Guid                                                      │
│ - Email: string                                                     │
│ - PasswordHash: string                                              │
│ - CreatedAt: DateTime                                               │
│ - UpdatedAt: DateTime                                               │
├─────────────────────────────────────────────────────────────────────┤
│ + Login()                                                           │
│ + Register()                                                        │
└─────────────────────────────────────────────────────────────────────┘

┌─────────────────────────────────────────────────────────────────────┐
│                       Product (Entity)                              │
├─────────────────────────────────────────────────────────────────────┤
│ - ProductId: Guid                                                   │
│ - Name: string                                                      │
│ - Description: string                                               │
│ - Price: decimal                                                    │
│ - Stock: int                                                        │
│ - CreatedAt: DateTime                                               │
│ - UpdatedAt: DateTime                                               │
├─────────────────────────────────────────────────────────────────────┤
│ + GetProductDetails()                                               │
│ + UpdateStock()                                                     │
└─────────────────────────────────────────────────────────────────────┘

┌─────────────────────────────────────────────────────────────────────┐
│                        Sale (Entity)                                │
├─────────────────────────────────────────────────────────────────────┤
│ - SaleId: Guid                                                      │
│ - UserId: Guid                                                      │
│ - SaleDate: DateTime                                                │
│ - TotalAmount: decimal                                              │
│ - Status: string                                                    │
│ - CreatedAt: DateTime                                               │
├─────────────────────────────────────────────────────────────────────┤
│ + CalculateTotalAmount()                                            │
│ + CancelSale()                                                      │
│ + SaleProducts: ICollection<SaleProduct>                            │
└─────────────────────────────────────────────────────────────────────┘
           │
           │ 1..* (One Sale to Many SaleProducts)
           │
┌─────────────────────────────────────────────────────────────────────┐
│                    SaleProduct (Entity)                             │
├─────────────────────────────────────────────────────────────────────┤
│ - SaleProductId: Guid                                               │
│ - SaleId: Guid (Foreign Key)                                        │
│ - ProductId: Guid (Foreign Key)                                     │
│ - Quantity: int                                                     │
│ - UnitPrice: decimal                                                │
│ - Subtotal: decimal                                                 │
├─────────────────────────────────────────────────────────────────────┤
│ + CalculateSubtotal()                                               │
│ + Sale: Sale (Navigation)                                           │
│ + Product: Product (Navigation)                                     │
└─────────────────────────────────────────────────────────────────────┘
```

## Database Schema (DER)

```
┌──────────────────┐
│      Users       │
├──────────────────┤
│ PK  UserId       │───┐
│     Email        │   │
│     PasswordHash │   │
│     Role         │   │
│     CreatedAt    │   │
│     UpdatedAt    │   │
└──────────────────┘   │
                       │ 1..* (One User to Many Sales)
                       │
                    ┌──────────────────┐
                    │      Sales       │
                    ├──────────────────┤
                    │ PK  SaleId       │───┐
                    │ FK  UserId       │   │
                    │     SaleDate     │   │
                    │     TotalAmount  │   │
                    │     Status       │   │
                    │     CreatedAt    │   │
                    └──────────────────┘   │
                                          │ 1..*
                                          │
                          ┌───────────────┴────────────────┐
                          │                                │
                     ┌────────────────┐          ┌─────────────────┐
                     │  SaleProducts  │          │    Products     │
                     ├────────────────┤          ├─────────────────┤
                     │ PK SaleProduct │          │ PK ProductId    │
                     │    Id          │          │    Name         │
                     │ FK SaleId      │──┐    ┌──    Description  │
                     │ FK ProductId   │──┼────┤     Price        │
                     │    Quantity    │  │     │    Stock        │
                     │    UnitPrice   │  │     │    CreatedAt    │
                     │    Subtotal    │  │     │    UpdatedAt    │
                     └────────────────┘  │     └─────────────────┘
                                         │
                                    (Many-to-Many)
                                    Junction Table
```

## Installation

### Prerequisites

- .NET 9.0 or higher
- PostgreSQL 16 or higher
- Visual Studio 2022 or Visual Studio Code

### Steps

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd Firmeza.API
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Use .ENV**
   - You need the .env file to load all credentials that program needs.

4. **Apply migrations**
   ```bash
   dotnet ef database update
   ```

5. **Run the application**
   ```bash
   dotnet run
   ```

## Configuration

### Environment Variables (.env)

Create a `.env` file in the root directory:

```
DB_HOST=""
DB_PORT=""
DB_NAME=""
DB_USER=""
DB_PASS=""
JWT_KEY=""
JWT_ISSUER=""
JWT_AUDIENCE=""
SMTP_HOST=""
SMTP_PORT=""
SMTP_USER=""
SMTP_PASSWORD=""
SMTP_FROM=tucuenta@gmail.com

```

## Usage

### Running the Application

```bash
# Development mode
dotnet run --launch-profile https

# Production mode
dotnet publish -c Release
dotnet Firmeza.API.dll
```

### API Testing

Use the included `Firmeza.API.http` file for REST Client testing:

```http
### Register New User
POST https://localhost:5152/api/auth/register
Content-Type: application/json

{
  "email": "user@example.com",
  "password": "SecurePassword123!",
  "firstName": "John",
  "lastName": "Doe"
}

### Login
POST https://localhost:5152/api/auth/login
Content-Type: application/json

{
  "email": "user@example.com",
  "password": "SecurePassword123!"
}

### Get All Products
GET https://localhost:5152/api/products
Authorization: Bearer {token}

### Create Product
POST https://localhost:5152/api/products
Authorization: Bearer {token}
Content-Type: application/json

{
  "name": "Product Name",
  "description": "Product Description",
  "price": 99.99,
  "stock": 100
}
```

## API Endpoints

### Authentication
- `POST /api/auth/register` - Register a new user
- `POST /api/auth/login` - User login
- `POST /api/auth/refresh-token` - Refresh JWT token

### Products
- `GET /api/products` - Get all products
- `GET /api/products/{id}` - Get product by ID
- `POST /api/products` - Create new product (Admin only)
- `PUT /api/products/{id}` - Update product (Admin only)
- `DELETE /api/products/{id}` - Delete product (Admin only)

### Sales
- `GET /api/sales` - Get user sales
- `GET /api/sales/{id}` - Get sale details
- `POST /api/sales` - Create new sale
- `PUT /api/sales/{id}` - Update sale status
- `DELETE /api/sales/{id}` - Cancel sale

### Sale Products
- `GET /api/sales/{saleId}/products` - Get products in a sale
- `POST /api/sales/{saleId}/products` - Add product to sale
- `DELETE /api/sales/{saleId}/products/{productId}` - Remove product from sale

## Response Format

### Success Response
```json
{
  "success": true,
  "message": "Operation completed successfully",
  "data": {
    "id": "123e4567-e89b-12d3-a456-426614174000",
    "name": "Product Name"
  },
  "timestamp": "2024-01-15T10:30:00Z"
}
```

### Error Response
```json
{
  "success": false,
  "message": "An error occurred",
  "errors": [
    "Error detail 1",
    "Error detail 2"
  ],
  "timestamp": "2024-01-15T10:30:00Z"
}
```

## Technologies

| Technology | Purpose |
|-----------|---------|
| **ASP.NET Core 9** | Web framework |
| **Entity Framework Core** | ORM and database access |
| **SQL Server** | Relational database |
| **JWT** | Authentication and authorization |
| **AutoMapper** | Object mapping |
| **SMTP** | Email notifications |
| **Docker** | Containerization |
| **Swagger/OpenAPI** | API documentation |

## Project Structure Details

### Controllers
- **AuthController** - Handles user registration, login, and token management
- **ProductController** - Manages product CRUD operations
- **SaleController** - Handles sales transactions
- **SaleProductController** - Manages product-sale relationships

### Services
Business logic layer containing:
- Authentication service
- Product service
- Sales service
- Email notification service

### Repositories
Data access abstraction implementing the Repository pattern:
- Product repository
- Sale repository
- User repository

### DTOs (Data Transfer Objects)
- `LoginDTO` - User login credentials
- `RegisterDTO` - User registration data
- `ProductDTO` - Product transfer object
- `SaleDTO` - Sale transfer object

### Configurations
- `DatabaseConfig` - Database connection settings
- `JwtConfigs` - JWT authentication configuration
- `SmtpCredentials` - Email service configuration

## Deployment

### Docker Deployment

```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Firmeza.API.csproj", "."]
RUN dotnet restore "Firmeza.API.csproj"
COPY . .
RUN dotnet build "Firmeza.API.csproj" -c Release -o /app/build

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app/build .
EXPOSE 80
ENTRYPOINT ["dotnet", "Firmeza.API.dll"]
```

Build and run:
```bash
docker build -t firmeza-api:latest .
docker run -p 80:80 -e DefaultConnection="your-connection-string" firmeza-api:latest
```

## Error Handling

The API implements comprehensive error handling with appropriate HTTP status codes:

- `200 OK` - Successful request
- `201 Created` - Resource created successfully
- `400 Bad Request` - Invalid request data
- `401 Unauthorized` - Missing or invalid authentication
- `403 Forbidden` - Insufficient permissions
- `404 Not Found` - Resource not found
- `500 Internal Server Error` - Server-side error

## Security Considerations

✅ **JWT Authentication** - Secure token-based authentication  
✅ **Password Hashing** - Passwords are hashed using bcrypt  
✅ **HTTPS Only** - All production endpoints use HTTPS  
✅ **CORS Configuration** - Configured for allowed origins  
✅ **SQL Injection Prevention** - Using parameterized queries via EF Core  
✅ **Rate Limiting** - Prevent abuse through request throttling  

## Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit changes (`git commit -m 'Add AmazingFeature'`)
4. Push to branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Support

For issues, questions, or suggestions, please create an issue in the repository or contact the development team.

---