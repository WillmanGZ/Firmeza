# AdminManager.Web

**Administrative and Sales Management System - ASP.NET Core Razor Pages Web Application**

AdminManager.Web is a modern web application designed to manage products, sales, and generate PDF reports. It provides an intuitive interface for administrators and salespeople, with secure authentication based on ASP.NET Identity.

---

## 📋 Table of Contents

- [Key Features](#key-features)
- [Prerequisites](#prerequisites)
- [Technologies Used](#technologies-used)
- [Project Structure](#project-structure)
- [Database Schema](#database-schema)
- [Class Diagram](#class-diagram)
- [Local Installation](#local-installation)
- [Configuration](#configuration)
- [Local Execution](#local-execution)
- [Docker Deployment](#docker-deployment)
- [Application Usage](#application-usage)
- [Detailed Features](#detailed-features)
- [Troubleshooting](#troubleshooting)

---

## ✨ Key Features

### 🔐 Authentication and Authorization
- Secure login system with ASP.NET Identity
- User roles (Admin, Salesperson)
- Credential management with secure hashing
- Persistent sessions

### 📦 Product Management
- Complete CRUD for products
- Catalog visualization
- Price management
- Detailed item descriptions

### 💰 Sales Management
- Sales transaction recording
- Product association with sales
- Automatic total amount calculation
- Sales history

### 📄 Report Generation
- Sales export to PDF
- Consolidated reports
- Professional documents
- Direct download from browser

### 📊 Administration Dashboard
- Dashboard with statistics
- Quick access to functionalities
- Key data visualization
- Centralized management

### 📈 Excel Import
- Bulk data upload
- Automatic validation
- Column mapping
- Detailed error reports

---

## 📋 Prerequisites

### Required Software

| Component | Minimum Version | Description |
|-----------|-----------------|-----------|
| .NET SDK | 9.0 | Runtime and .NET compiler |
| SQL Server / PostgreSQL | 2019+ | Database |
| Visual Studio / VS Code | 2022+ | Code editor |
| Git | 2.30+ | Version control |
| Docker (optional) | 20.10+ | Containers |


## 🛠️ Technologies Used

### Backend Framework
- **ASP.NET Core 9.0**: Modern high-performance web framework
- **Razor Pages**: Page-oriented programming model
- **C# 13**: Compiled programming language

### Database
- **Entity Framework Core 9.0**: ORM for data access
- **PostgreSQL**: Database engines
- **Migrations**: Database schema versioning

### Authentication
- **ASP.NET Identity**: Integrated identity system
- **Identity EntityFrameworkCore**: EF Core integration

### Report Generation
- **QuestPDF**: Library for creating professional PDFs
- **EPPlus (8.2.1)**: Excel file reading and writing

### Utilities
- **DotNetEnv (3.1.1)**: Environment variable handling
- **Bootstrap 5**: Responsive CSS framework

---

## 📁 Project Structure

```
AdminManager.Web/
│
├── 📄 Program.cs                          # Entry point and configuration
├── 📄 AdminManager.Web.csproj             # Project file
├── 📄 appsettings.json                    # General configuration
├── 📄 appsettings.Development.json        # Development configuration
├── 📄 README.md                           # This file
│
├── 📁 Data/                               # Data access layer
│   ├── AppDbContext.cs                    # Database context
│   ├── AppDbContextFactory.cs             # Factory for migrations
│   ├── 📁 Entities/                       # Data models
│   │   ├── Product.cs                     # Product entity
│   │   ├── Sale.cs                        # Sale entity
│   │   └── SaleProduct.cs                 # Sale-Product entity (M:M relationship)
│   └── 📁 Seeders/                        # Data initializers
│       └── IdentitySeed.cs                # Identity initial data
│
├── 📁 Services/                           # Business logic services
│   ├── ExcelImportService.cs              # Excel file import
│   └── AllSalesPdfService.cs              # PDF sales report generation
│
├── 📁 Pages/                              # Razor pages
│   ├── 📁 Admin/                          # Administration section
│   │   ├── Dashboard.cshtml               # Control panel
│   │   ├── Dashboard.cshtml.cs            # Dashboard code-behind
│   │   ├── Sales.cshtml                   # Sales management
│   │   ├── Sales.cshtml.cs                # Sales code-behind
│   │   ├── 📁 Products/                   # Product management
│   │   │   ├── Index.cshtml
│   │   │   ├── Create.cshtml
│   │   │   ├── Edit.cshtml
│   │   │   └── Delete.cshtml
│   │   └── 📁 Clients/                    # Client management
│   │
│   ├── 📁 Excel/                          # Excel functionalities
│   │   ├── Import.cshtml                  # Import page
│   │   ├── Import.cshtml.cs               # Import code-behind
│   │   ├── DownloadTemplate.cshtml        # Template download
│   │   └── DownloadTemplate.cshtml.cs     # Download code-behind
│   │
│   ├── 📁 Shared/                         # Shared components
│   │   ├── _Layout.cshtml                 # Master layout
│   │   ├── _Layout.cshtml.css             # Layout styles
│   │   ├── _LoginPartial.cshtml           # Login component
│   │   └── _ValidationScriptsPartial.cshtml # Validation scripts
│   │
│   ├── Index.cshtml                       # Home page
│   ├── Index.cshtml.cs
│   ├── Privacy.cshtml                     # Privacy page
│   ├── Privacy.cshtml.cs
│   ├── Error.cshtml                       # Error page
│   ├── Error.cshtml.cs
│   ├── _ViewImports.cshtml
│   ├── _ViewStart.cshtml
│   └── 📁 Areas/Identity/                 # Identity pages
│       └── Pages/Account/                 # Login, registration, recovery
│
├── 📁 Models/                             # View models
│   ├── AllSalesPdfDocument.cs             # Model for sales PDF
│   └── ReceiptPdfModel.cs                 # Model for receipts
│
├── 📁 Configs/                            # Configuration
│   └── DatabaseConfig.cs                  # Database extensions
│
├── 📁 Migrations/                         # Database migrations
│   ├── 20251028161421_InitialCreate.cs
│   ├── 20251029212335_AllTables.cs
│   ├── 20251030001355_IdentityTables.cs
│   └── AppDbContextModelSnapshot.cs
│
├── 📁 Properties/                         # Project properties
│   └── launchSettings.json                # Launch configuration
│
├── 📁 wwwroot/                            # Static files
│   ├── 📁 css/
│   │   └── site.css                       # Custom styles
│   ├── 📁 js/
│   │   └── site.js                        # Custom scripts
│   └── 📁 lib/                            # Third-party libraries
│       ├── bootstrap/
│       ├── jquery/
│       ├── jquery-validation/
│       └── jquery-validation-unobtrusive/
│
├── 📁 Areas/                              # Identity areas
│   └── Identity/
│       └── Pages/
│           └── Account/                   # Authentication pages
│
└── 📁 bin/                                # Compiled binaries
    └── 📁 obj/                            # Intermediate objects
```

---

## 🗄️ Database Schema

### Entity-Relationship Diagram (ER)

```
┌─────────────────────────────────────────────────────────────┐
│                   AspNetUsers (Identity)                    │
├─────────────────────────────────────────────────────────────┤
│ PK: Id                                                      │
│ UserName          (unique)                                  │
│ Email             (unique)                                  │
│ NormalizedUserName                                          │
│ NormalizedEmail                                             │
│ EmailConfirmed                                              │
│ PasswordHash                                                │
│ SecurityStamp                                               │
│ ConcurrencyStamp                                            │
│ PhoneNumber                                                 │
│ PhoneNumberConfirmed                                        │
│ TwoFactorEnabled                                            │
│ LockoutEnd                                                  │
│ LockoutEnabled                                              │
│ AccessFailedCount                                           │
└─────────────────────────────────────────────────────────────┘
         │
         │ (1:N) - Client
         │
         ▼
┌──────────────────────────┐    ┌─────────────────────────────┐
│        Sale              │    │      Product                │
├──────────────────────────┤    ├─────────────────────────────┤
│ PK: Id (Guid)            │    │ PK: Id (Guid)               │
│ Date                     │    │ Name (100 chars, required)  │
│ FK: ClientId             │    │ Description (255 chars)     │
│ CreatedAt (default: now) │    │ Price (required, 0-100M)    │
└──────────────────────────┘    └─────────────────────────────┘
         │                               │
         │ (N:M) through                 │
         │ SaleProduct                   │
         └────────────────┬──────────────┘
                          │
                          ▼
             ┌──────────────────────────────┐
             │      SaleProduct             │
             ├──────────────────────────────┤
             │ PK: Id (Guid)                │
             │ FK: SaleId                   │
             │ FK: ProductId                │
             │ Quantity (1-1000)            │
             │ UnitPrice (0-100M)           │
             └──────────────────────────────┘
```

### Entity Descriptions

#### **Product**
- Catalog of available items for sale
- Each product has price, name, and description
- Can be associated with multiple sales

| Field | Type | Constraints |
|-------|------|-------------|
| Id | Guid | PK, Default: NewGuid() |
| Name | string | Required, MaxLength: 100 |
| Description | string | MaxLength: 255 |
| Price | int | Required, Range: 0-100000000 |

#### **Sale**
- Sale transaction linked to a customer
- Contains multiple products through SaleProduct
- Record with date and customer

| Field | Type | Constraints |
|-------|------|-------------|
| Id | Guid | PK, Default: NewGuid() |
| Date | DateTime | Required, Default: DateTime.Now |
| ClientId | string | Required, FK → AspNetUsers |
| Client | IdentityUser | Navigation property |
| SaleProducts | Collection | N:M relationship |

#### **SaleProduct**
- Many-to-Many join table between Sale and Product
- Defines quantity and unit price at time of sale
- Allows price history

| Field | Type | Constraints |
|-------|------|-------------|
| Id | Guid | PK, Default: NewGuid() |
| SaleId | Guid | Required, FK → Sale |
| Sale | Sale | Navigation property |
| ProductId | Guid | Required, FK → Product |
| Product | Product | Navigation property |
| Quantity | int | Required, Range: 1-1000 |
| UnitPrice | int | Required, Range: 0-100000000 |

#### **AspNetUsers** (Users - Identity)
- System users (includes customers and administrators)
- Centralized identity management
- Secure credential storage

---

## 🏗️ Class Diagram

```
┌────────────────────────────────────────────────────────┐
│                   Program.cs                           │
│  ┌──────────────────────────────────────────────────┐  │
│  │ • AddDatabase()                                  │  │
│  │ • AddIdentity<IdentityUser, IdentityRole>()     │  │
│  │ • AddScoped<ExcelImportService>()               │  │
│  │ • AddScoped<AllSalesPdfService>()               │  │
│  │ • AddRazorPages()                               │  │
│  └──────────────────────────────────────────────────┘  │
└────────────────────────────────────────────────────────┘
                          ▲
                          │ uses
        ┌─────────────────┼─────────────────┐
        │                 │                 │
        ▼                 ▼                 ▼
    ┌──────────┐  ┌─────────────────┐  ┌──────────────────┐
    │AppDbCtx  │  │IdentitySeed     │  │ DependencyInjection
    ├──────────┤  ├─────────────────┤  │ Configuration
    │ OnConfig │  │ • SeedAsync()   │  └──────────────────┘
    │uring()  │  │ • CreateRoles() │
    │ OnModel  │  │ • CreateAdmin() │
    │Creating()   └─────────────────┘
    └──────────┘


┌────────────────────────────────────────────────────────┐
│              Services Layer                            │
├────────────────────────────────────────────────────────┤
│                                                        │
│  ┌──────────────────────┐  ┌───────────────────────┐ │
│  │ExcelImportService    │  │AllSalesPdfService     │ │
│  ├──────────────────────┤  ├───────────────────────┤ │
│  │ • ImportAsync()      │  │ • GeneratePdfAsync()  │ │
│  │ • ValidateData()     │  │ • BuildDocument()     │ │
│  │ • ParseExcel()       │  │ • ConfigureStyle()    │ │
│  └──────────────────────┘  └───────────────────────┘ │
│         │                            │                │
│         │ uses EPPlus               │ uses QuestPDF │
│         └───────┬────────────────────┘                │
│                 │                                    │
│                 ▼                                    │
│         ┌─────────────────┐                         │
│         │   AppDbContext  │                         │
│         └─────────────────┘                         │
└────────────────────────────────────────────────────────┘


┌────────────────────────────────────────────────────────┐
│              Presentation Layer (Razor Pages)          │
├────────────────────────────────────────────────────────┤
│                                                        │
│  ┌──────────────┐         ┌──────────────────────┐    │
│  │Index.cshtml  │         │ Admin/Dashboard      │    │
│  └──────────────┘         ├──────────────────────┤    │
│         │                 │ • OnGet()            │    │
│         │                 │ • LoadStatistics()   │    │
│         │                 └──────────────────────┘    │
│         │                          │                 │
│  ┌──────────────────────────┐     │                 │
│  │ Areas/Identity/Account   │     │                 │
│  ├──────────────────────────┤     │                 │
│  │ • Login.cshtml           │     │                 │
│  │ • Register.cshtml        │     │                 │
│  │ • Logout.cshtml          │     │                 │
│  └──────────────────────────┘     │                 │
│                                   │                 │
│  ┌─────────────────────┐          │                 │
│  │ Admin/Sales         │          │                 │
│  ├─────────────────────┤          │                 │
│  │ • Index()           │◄─────────┘                 │
│  │ • OnPost()          │                             │
│  │ • OnPostCreate()    │                             │
│  └─────────────────────┘                             │
│           │                                          │
│           ├─ uses ExcelImportService                 │
│           └─ uses AllSalesPdfService                 │
│                                                      │
│  ┌──────────────────────────┐                        │
│  │ Admin/Products           │                        │
│  ├──────────────────────────┤                        │
│  │ • Index() - List         │                        │
│  │ • Create() - Create      │                        │
│  │ • Edit() - Edit          │                        │
│  │ • Delete() - Delete      │                        │
│  └──────────────────────────┘                        │
│                                                      │
│  ┌──────────────────────────┐                        │
│  │ Excel/Import             │                        │
│  ├──────────────────────────┤                        │
│  │ • OnGet()                │                        │
│  │ • OnPostAsync()          │                        │
│  │ uses ExcelImportService  │                        │
│  └──────────────────────────┘                        │
│                                                      │
│  ┌──────────────────────────┐                        │
│  │ Excel/DownloadTemplate   │                        │
│  ├──────────────────────────┤                        │
│  │ • OnPostAsync()          │                        │
│  │ Download Excel template  │                        │
│  └──────────────────────────┘                        │
│                                                      │
│  ┌──────────────────────────┐                        │
│  │ Shared/                  │                        │
│  ├──────────────────────────┤                        │
│  │ • _Layout.cshtml         │ (Master page)         │
│  │ • _LoginPartial.cshtml   │ (Login component)     │
│  │ • _ValidationScriptsPartial.cshtml               │
│  └──────────────────────────┘                        │
│                                                      │
└────────────────────────────────────────────────────────┘


┌────────────────────────────────────────────────────────┐
│                  Data Layer                            │
├────────────────────────────────────────────────────────┤
│                                                        │
│  ┌──────────────────────┐                             │
│  │  AppDbContext        │                             │
│  ├──────────────────────┤                             │
│  │ DbSet<Product>       │                             │
│  │ DbSet<Sale>          │                             │
│  │ DbSet<SaleProduct>   │                             │
│  │                      │                             │
│  │ OnConfiguring()      │                             │
│  │ OnModelCreating()    │                             │
│  └──────────────────────┘                             │
│           │                                            │
│           │ inherits                                   │
│           ▼                                            │
│  ┌─────────────────────────────────┐                  │
│  │ IdentityDbContext               │                  │
│  │ (Microsoft.AspNetCore.Identity) │                  │
│  └─────────────────────────────────┘                  │
│                                                        │
│  Entities:                                             │
│  ├── Product.cs        (Products)                     │
│  ├── Sale.cs           (Sales)                        │
│  └── SaleProduct.cs    (Sale-Product)                │
│                                                        │
└────────────────────────────────────────────────────────┘
```

---

## 🚀 Local Installation

### Step 1: Clone the Repository

```powershell
# Navigate to desired folder
cd C:\Users\USUARIO\OneDrive\Documentos\Projects\Firmeza

# Clone repository
git clone https://github.com/WillmanGZ/Firmeza.git
cd AdminManager.Web
```

### Step 2: Restore Dependencies

```powershell
# Restore NuGet packages
dotnet restore
```

**Expected output:**
```
Determining projects to restore...
Restore complete in XXXX ms for C:\...\AdminManager.Web.csproj.
```

### Step 3: Verify Installation

```powershell
# Verify .NET version
dotnet --version

# List solution
dotnet sln list
```

### Step 4: Build the Project

```powershell
# Compile in Debug mode
dotnet build

# Or compile in Release mode
dotnet build -c Release
```

---

## ⚙️ Configuration

### Database

#### File: `appsettings.json`

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

#### File: `appsettings.Development.json` (create if not exists)

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=AdminManagerDb;Integrated Security=true;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Debug",
      "Microsoft": "Warning"
    }
  }
}
```

#### File: `appsettings.Production.json` (for production)

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=your_server;Database=AdminManagerDb;User Id=your_user;Password=your_password;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Warning"
    }
  }
}
```

### Database Configuration (DatabaseConfig.cs)

```csharp
// Extension to inject database
public static IServiceCollection AddDatabase(this IServiceCollection services, 
    IConfiguration configuration)
{
    var connectionString = configuration.GetConnectionString("DefaultConnection");
    
    // SQL Server
    services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(connectionString));
    
    // Or PostgreSQL (if preferred)
    // services.AddDbContext<AppDbContext>(options =>
    //     options.UseNpgsql(connectionString));
    
    return services;
}
```

### Environment Variables (Optional)

Create `.env` file in project root:

```env
ASPNETCORE_ENVIRONMENT=Development
DOTNET_ENVIRONMENT=Development
ConnectionStrings__DefaultConnection=Server=(localdb)\\mssqllocaldb;Database=AdminManagerDb;Integrated Security=true;
```

### License Dependencies

License configuration required for commercial libraries:

```csharp
// In Program.cs
ExcelPackage.License.SetNonCommercialPersonal("FirmezaApp");
QuestPDF.Settings.License = LicenseType.Community;
```

---

## ▶️ Local Execution

### Option 1: Using .NET CLI

```powershell
# Run application (compiles and runs)
dotnet run

# Run without compiling (if already compiled)
dotnet run --no-build
```

**Expected output:**
```
Building...
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: https://localhost:7001
      Now listening on: http://localhost:5147
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to exit.
```

### Option 2: Using Visual Studio 2022

1. Open `AdminManager.Web.csproj` in Visual Studio
2. Wait for projects to load
3. Select `AdminManager.Web` as startup project (right-click → "Set as Startup Project")
4. Press `F5` or `Ctrl+F5` (without debugging)
5. Opens automatically at `https://localhost:7001`

### Option 3: Using Visual Studio Code

```powershell
# Open VS Code in project folder
code .

# Or if already open, create launch configuration
# Go to: Debug → Add Configuration → .NET Core
```

Create/Verify `.vscode/launch.json`:

```json
{
    "version": "0.2.0",
    "configurations": [
        {
            "name": ".NET Core Launch (web)",
            "type": "coreclr",
            "request": "launch",
            "preLaunchTask": "build",
            "program": "${workspaceFolder}/bin/Debug/net9.0/AdminManager.Web.dll",
            "args": [],
            "cwd": "${workspaceFolder}",
            "stopAtEntry": false,
            "serverReadyAction": {
                "action": "openExternally",
                "pattern": "\\bNow listening on:\\s+(https?://\\S+)"
            },
            "env": {
                "ASPNETCORE_ENVIRONMENT": "Development"
            },
            "console": "integratedTerminal",
            "internalConsoleOptions": "neverOpen"
        }
    ]
}
```

### First Execution

When you run for the first time:

1. **Application will automatically create the database**
2. **All migrations will run**
3. **Default roles will be created (Admin, User)**
4. **Default admin account will be created:**
   - Email: `admin@example.com`
   - Password: `Admin@123456`

Access at: `https://localhost:7001`

---

## 🐳 Docker Deployment

### Requirement: Docker Desktop Installed

Download from: https://www.docker.com/products/docker-desktop

### Step 1: Create Dockerfile

Create `Dockerfile` file in project root:

```dockerfile
# Stage 1: Compilation
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["AdminManager.Web.csproj", "./"]
RUN dotnet restore "AdminManager.Web.csproj"
COPY . .
RUN dotnet build "AdminManager.Web.csproj" -c Release -o /app/build

# Stage 2: Publication
FROM build AS publish
RUN dotnet publish "AdminManager.Web.csproj" -c Release -o /app/publish

# Stage 3: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 80 443
ENV ASPNETCORE_URLS=http://+:80;https://+:443
ENV ASPNETCORE_ENVIRONMENT=Production
ENTRYPOINT ["dotnet", "AdminManager.Web.dll"]
```

### Step 2: Create docker-compose.yml

```yaml
version: '3.8'

services:
  # Database
  mssql:
    image: mcr.microsoft.com/mssql/server:2019-latest
    container_name: adminmanager-db
    environment:
      ACCEPT_EULA: "Y"
      SA_PASSWORD: "YourPassword@123"
      MSSQL_PID: "Express"
    ports:
      - "1433:1433"
    volumes:
      - sqlserver_data:/var/opt/mssql
    networks:
      - adminmanager-network
    healthcheck:
      test: /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "YourPassword@123" -Q "SELECT 1"
      interval: 10s
      timeout: 5s
      retries: 5

  # Web Application
  web:
    build:
      context: .
      dockerfile: Dockerfile
    container_name: adminmanager-web
    environment:
      ConnectionStrings__DefaultConnection: "Server=mssql;Database=AdminManagerDb;User=sa;Password=YourPassword@123;TrustServerCertificate=true;"
      ASPNETCORE_ENVIRONMENT: "Production"
      ASPNETCORE_URLS: "http://+:80;https://+:443"
    ports:
      - "8080:80"
      - "8443:443"
    depends_on:
      mssql:
        condition: service_healthy
    networks:
      - adminmanager-network
    volumes:
      - ./wwwroot:/app/wwwroot

volumes:
  sqlserver_data:

networks:
  adminmanager-network:
    driver: bridge
```

### Step 3: Build Docker Image

```powershell
# Build image
docker build -t adminmanager-web:latest .

# See created image
docker images | findstr adminmanager
```

### Step 4: Run with Docker Compose

```powershell
# Start services (database and application)
docker-compose up -d

# View application logs
docker-compose logs -f web

# See service status
docker-compose ps

# Stop services
docker-compose down

# Delete volumes (warning: deletes data)
docker-compose down -v
```

### Application Access in Docker

- **Web**: `http://localhost:8080`
- **Database**: `localhost:1433`

### Useful Docker Commands

```powershell
# See running containers
docker ps

# Access container terminal
docker exec -it adminmanager-web bash

# View service logs
docker-compose logs web

# Clean up stopped containers
docker container prune

# Clean up unused images
docker image prune
```

### Docker Troubleshooting

```powershell
# Container keeps restarting
docker logs adminmanager-web

# Issue: Connection refused
# Verify database is ready before application starts

# Rebuild without cache
docker-compose build --no-cache

# Restart services
docker-compose restart
```

---

## 📖 Application Usage

### First Access

1. Navigate to `https://localhost:7001` (local) or `http://localhost:8080` (Docker)
2. Click **"Login"** in top right corner
3. Use default credentials:
   - **Email**: `admin@example.com`
   - **Password**: `Admin@123456`

> ⚠️ **IMPORTANT**: Change these credentials in production

### Administration Panel

Once authenticated, access:

- **Dashboard** (`/Admin/Dashboard`): Summary of statistics and activity
- **Products** (`/Admin/Products`): Catalog management
- **Sales** (`/Admin/Sales`): Transaction recording
- **Clients** (`/Admin/Clients`): Client management

### Typical Workflow

#### 1. Create Products

1. Go to **Admin → Products**
2. Click **"New Product"**
3. Complete:
   - Name (max. 100 characters)
   - Description (max. 255 characters)
   - Price
4. Click **"Save"**

#### 2. Record a Sale

1. Go to **Admin → Sales**
2. Click **"New Sale"**
3. Complete:
   - Select customer
   - Select product(s)
   - Enter quantity
4. System automatically calculates total
5. Click **"Register Sale"**

#### 3. Generate PDF Reports

1. Go to **Admin → Sales**
2. Click **"Download PDF"** or **"Generate Report"**
3. PDF file with all sales downloads

#### 4. Import Data from Excel

1. Go to **Excel → Import**
2. Download example template
3. Fill with your data
4. Select file and click **"Import"**
5. System validates and confirms

---

## 💡 Detailed Features

### 🔐 Authentication

#### New User Registration

```
POST: /Areas/Identity/Pages/Account/Register
```

- Unique email validation
- Password confirmation
- Secure password hashing (bcrypt)

#### Login

```
POST: /Areas/Identity/Pages/Account/Login
```

- Session persistence
- Password recovery
- Lockout after multiple failed attempts

### 📦 Product Management

#### Complete CRUD

| Operation | Endpoint | Description |
|-----------|----------|-------------|
| List | GET `/Admin/Products` | View all products |
| Create | POST `/Admin/Products/Create` | Add new product |
| Edit | PUT `/Admin/Products/Edit/{id}` | Modify product |
| Delete | DELETE `/Admin/Products/Delete/{id}` | Delete product |

#### Validations

- Name required and unique
- Price must be positive
- Description maximum 255 characters

### 💰 Sales Management

#### Sale Registration

```csharp
Sale sale = new Sale
{
    Date = DateTime.Now,
    ClientId = clientId,
    SaleProducts = new List<SaleProduct>
    {
        new SaleProduct
        {
            ProductId = productId,
            Quantity = 5,
            UnitPrice = 99999
        }
    }
};

await context.Sales.AddAsync(sale);
await context.SaveChangesAsync();
```

#### Total Calculation

System automatically calculates:
- Subtotal per product: `Quantity × UnitPrice`
- Sale total: Sum of subtotals

### 📄 PDF Generation

#### Service: `AllSalesPdfService.cs`

```csharp
public async Task<byte[]> GeneratePdfAsync()
{
    var sales = await context.Sales.Include(s => s.SaleProducts).ToListAsync();
    // Document generated with QuestPDF
    var pdf = document.GeneratePdf();
    return pdf;
}
```

#### Features

- Header with logo and date
- Table with all sales
- Total calculation
- Footer with information

### 📊 Excel Import

#### Service: `ExcelImportService.cs`

```csharp
public async Task<ImportResult> ImportAsync(IFormFile file)
{
    using var package = new ExcelPackage(file.OpenReadStream());
    var worksheet = package.Workbook.Worksheets[0];
    
    // Validate data
    // Map columns
    // Insert into database
    
    return new ImportResult { Success = true, RowsImported = count };
}
```

#### Excel Template

Columns should be:

| Name | Description | Price | Stock |
|------|-------------|-------|-------|
| Laptop | Portable Computer | 999999 | 10 |
| Mouse | Input Device | 25000 | 50 |

### 📊 Dashboard

Displays key statistics:

- Total registered products
- Total sales this month
- Total revenue
- Active customers
- Trend charts

---

## 🔧 Troubleshooting

### Error: "The database cannot be created because the specified path is invalid"

**Cause**: Problem with database path in LocalDB

**Solution:**

```powershell
# Delete all LocalDB instances
sqllocaldb delete mssqllocaldb

# Recreate
sqllocaldb create mssqllocaldb

# Start
sqllocaldb start mssqllocaldb
```

### Error: "A network-related or instance-specific error"

**Cause**: SQL Server not accessible

**Solution:**

```powershell
# Verify SQL Server is running
Get-Service -Name "MSSQLSERVER"

# If stopped, start it
Start-Service -Name "MSSQLSERVER"

# If using LocalDB
sqllocaldb start mssqllocaldb
```

### Error: "Microsoft.EntityFrameworkCore.DbUpdateException"

**Cause**: Database constraint violation

**Solution:**

```powershell
# Delete database and recreate
dotnet ef database drop -f
dotnet ef database update
```

### Error: "The type or namespace name 'AppDbContext' does not exist"

**Cause**: Dependencies not restored

**Solution:**

```powershell
# Clear NuGet cache
dotnet nuget locals all --clear

# Restore
dotnet restore

# Build
dotnet build
```

### Error generating PDF: "The operation cannot be performed because an appropriate license is not set"

**Cause**: QuestPDF requires license configuration

**Solution:**

Verify in `Program.cs`:

```csharp
QuestPDF.Settings.License = LicenseType.Community;
```

### Port 7001 already in use

**Cause**: Another application using same port

**Solution:**

```powershell
# Find which application uses the port
netstat -ano | findstr :7001

# Kill the process
taskkill /PID <PID> /F

# Or change port in launchSettings.json
```

### Docker error: "Connection refused when accessing database"

**Cause**: Application container connects before database is ready

**Solution:**

The `docker-compose.yml` already includes `healthcheck` that waits for SQL Server to be ready.

```powershell
# Rebuild
docker-compose down -v
docker-compose up --build
```

---

## 📚 Additional Documentation

### Important Files

- **Program.cs**: Startup configuration
- **appsettings.json**: Application configuration
- **AppDbContext.cs**: Database definition
- **Seeders/IdentitySeed.cs**: Initial data
- **Services/**: Business logic

### External References

- [ASP.NET Core Documentation](https://docs.microsoft.com/en-us/aspnet/core/)
- [Entity Framework Core Docs](https://docs.microsoft.com/en-us/ef/core/)
- [ASP.NET Identity](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity/)
- [QuestPDF Documentation](https://www.questpdf.com/)
- [EPPlus Documentation](https://epplussoftware.com/)

---

## 🤝 Contributing

To contribute to the project:

1. Fork the repository
2. Create feature branch (`git checkout -b feature/amazing-feature`)
3. Commit changes (`git commit -m 'Add amazing feature'`)
4. Push to branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

---

## 📄 License

This project is under the MIT License. See `LICENSE` file for details.

---

## 📞 Support

To report issues or request help:

- **GitHub Issues**: [Create Issue](https://github.com/WillmanGZ/Firmeza/issues)
- **Email**: your_email@example.com
- **Documentation**: See `/Docs` in repository

---

## 📝 Changelog

### v1.0.0 (November 2025)

- ✅ Authentication system with ASP.NET Identity
- ✅ Complete product CRUD
- ✅ Sales management with M:M relationship
- ✅ PDF report generation
- ✅ Excel data import
- ✅ Administrative dashboard
- ✅ Docker deployment

### Upcoming Features (v1.1.0)

- 📋 Inventory tracking
- 📊 Advanced charts
- 🔔 Real-time notifications
- 📱 Mobile app
- 🌍 Multi-language support

---

**Last Updated**: November 22, 2025  
**Version**: 1.0.0  
**Status**: Production ✅

