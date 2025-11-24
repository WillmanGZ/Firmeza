# Firmeza.Tests

## Overview

**Firmeza.Tests** is a comprehensive test suite project built with C# and .NET 9.0. This project contains automated tests for the Firmeza application, focusing on API integration testing and web application testing using modern testing frameworks and best practices.

## Project Structure

```
Firmeza.Tests/
├── ApiTests.cs                          # API endpoint tests
├── WebTests.cs                          # Web application tests
├── CustomWebApplicationFactory.cs       # Custom test factory configuration
├── Dockerfile                           # Docker containerization
├── Firmeza.Tests.csproj                 # Project file
├── Firmeza.Tests.csproj.user            # User-specific project settings
├── README.md                            # Project documentation
├── Properties/
│   └── launchSettings.json              # Launch configuration
├── bin/
│   └── Debug/net9.0/                    # Compiled binaries
└── obj/
    └── Debug/net9.0/                    # Build artifacts
```

## Technology Stack

- **Framework**: .NET 9.0
- **Language**: C#
- **Testing Framework**: xUnit / MSTest (inferred from structure)
- **Container**: Docker support included
- **Target**: Web API and Web Application testing

## Key Components

### ApiTests.cs

Contains test cases for API endpoints, validating:
- Request/response handling
- HTTP status codes
- Data serialization and deserialization
- Error handling and edge cases

### WebTests.cs

Includes tests for web application functionality:
- UI component interaction
- Page navigation
- Form submission and validation
- User workflow scenarios

### CustomWebApplicationFactory.cs

A custom factory for creating test instances of the web application:
- Provides isolated test environment
- Configures test-specific services
- Manages test database and dependencies
- Enables integration testing with realistic application context

## Getting Started

### Prerequisites

- .NET 9.0 SDK or later
- Visual Studio 2022 / Visual Studio Code
- Docker (optional, for containerized testing)

### Installation

1. Clone the repository:
```bash
git clone <repository-url>
cd Firmeza.Tests
```

2. Restore dependencies:
```bash
dotnet restore
```

3. Build the project:
```bash
dotnet build
```

### Running Tests

Execute all tests:
```bash
dotnet test
```

Run specific test file:
```bash
dotnet test --filter "ClassName=ApiTests"
```

Run with detailed output:
```bash
dotnet test --verbosity detailed
```

## Docker Support

The project includes Docker support for containerized test execution.

Build the Docker image:
```bash
docker build -t firmeza.tests .
```

Run tests in a container:
```bash
docker run firmeza.tests
```

## Project Configuration

### Launch Settings
Configuration file: `Properties/launchSettings.json`

Defines different launch profiles for various testing scenarios and environments.

### Build Configuration
- **Debug**: Unoptimized build with debugging symbols
- **Release**: Optimized production-ready build

## Dependencies

The project references:
- **AdminManager.Web**: The web application under test
- Standard .NET testing libraries and utilities
- Integration testing frameworks

## Test Organization

Tests are organized by functionality:
- **API Tests**: Validate REST endpoints and server-side logic
- **Web Tests**: Validate client-side functionality and user interactions

## Best Practices

This project follows testing best practices:
- ✅ Isolated test cases with no interdependencies
- ✅ Comprehensive coverage of critical paths
- ✅ Use of test factories for consistent setup
- ✅ Clear, descriptive test names
- ✅ Proper cleanup and teardown procedures
- ✅ Mock/stub external dependencies

## Continuous Integration

The project is structured for CI/CD integration:
- Docker support for containerized environments
- Standard .NET test output formats
- Compatible with popular CI/CD platforms (GitHub Actions, Azure DevOps, etc.)

## Output

After running tests, results are generated in:
- **Console output**: Real-time test execution feedback
- **bin/Debug/net9.0**: Compiled test assemblies and dependencies

## Contributing

When adding new tests:
1. Follow existing naming conventions
2. Place tests in appropriate test class files
3. Use descriptive test method names
4. Include XML documentation comments
5. Ensure tests are isolated and repeatable

## License

[Specify your license here]

## Support

For issues or questions regarding the test suite, please contact the development team or create an issue in the project repository.

---

**Last Updated**: November 2024  
**Target Framework**: .NET 9.0  
**Status**: Active Development
