# Firmeza

## Project Overview

Firmeza is a comprehensive web application solution built with modern technologies. The project includes multiple components designed to work together seamlessly: an Admin Manager web interface, a Client-facing web application, and a robust API backend.

## Architecture

The Firmeza project is organized into several key components:

- **AdminManager.Web** - Administrative dashboard and management portal
- **Client.Web** - Client-facing web application
- **Firmeza.API** - RESTful API backend service
- **Firmeza.Tests** - Comprehensive test suite

## Technology Stack

- **.NET Core** - Backend framework
- **ASP.NET** - Web framework
- **Docker** - Containerization
- **Docker Compose** - Multi-container orchestration

## Getting Started

### Prerequisites

- Docker
- Docker Compose
- Git

### Running the Project with Docker Compose

The **recommended and easiest way** to run this project is using Docker Compose. This ensures consistency across all environments and eliminates dependency issues.

#### Steps:

1. **Clone the repository** (if not already done)
   ```bash
   git clone <repository-url>
   cd Firmeza
   ```

2. **Start all services**
   ```bash
   docker-compose up -d
   ```

3. **View logs**
   ```bash
   docker-compose logs -f
   ```

4. **Stop all services**
   ```bash
   docker-compose down
   ```

### Accessing the Services

Once Docker Compose is running, access the services at:

- **Admin Manager Web** - `http://localhost:5041`
- **Client Web** - `http://localhost:5173`
- **API** - `http://localhost:5152`

*Note: Replace `<port>` with the actual ports configured in `docker-compose.yml`*

## Local Development (Alternative)

If you prefer to run locally without Docker:

1. Ensure you have .NET SDK installed
2. Navigate to the desired project directory
3. Run: `dotnet run`

## Project Structure

```
Firmeza/
├── AdminManager.Web/      # Admin management dashboard
├── Client.Web/            # Client application
├── Firmeza.API/           # Backend API
├── Firmeza.Tests/         # Unit and integration tests
├── docker-compose.yml     # Docker Compose configuration
└── README.md              # This file
```

## Configuration

Each component has its own configuration files:

- `appsettings.json` - Production settings
- `appsettings.Development.json` - Development settings
- `.env` - Environment variables (if needed)

## Testing

Run the test suite:

```bash
dotnet test Firmeza.Tests/Firmeza.Tests.csproj
```

## Docker Compose Benefits

Using Docker Compose for this project provides:

✅ **Consistency** - Same environment across development, testing, and production  
✅ **Isolation** - Each service runs in its own container  
✅ **Simplicity** - Single command to start all services  
✅ **No Dependencies** - No need to install .NET SDK or other dependencies locally  
✅ **Easy Scaling** - Quickly manage multiple service instances  

## Contributing

1. Create a feature branch
2. Make your changes
3. Run tests to ensure everything works
4. Submit a pull request


## Support

For issues or questions, please open an issue in the repository.


## Links
Repo: https://github.com/WillmanGZ/Firmeza
Willman Alfredo Giraldo Zambrano
Clan Caiman