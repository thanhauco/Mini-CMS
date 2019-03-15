# MiniCMS

A modern, lightweight headless Content Management System built with ASP.NET Core.

## Overview

MiniCMS is an open-source headless CMS that provides a powerful REST API for managing structured content. It separates content from presentation, allowing developers to deliver content to any platform - web, mobile, IoT, or any other application.

## Features

- **Headless Architecture** - API-first design for maximum flexibility
- **Dynamic Schemas** - Define custom content types with various field types
- **Multi-tenant** - Support for multiple apps/projects
- **RESTful API** - Full CRUD operations with OData query support
- **Asset Management** - Upload and manage media files
- **Authentication** - JWT and API key support
- **Real-time Updates** - SignalR integration for live content updates
- **Webhooks** - Event-driven integrations

## Tech Stack

- **Framework**: ASP.NET Core 3.0
- **Database**: PostgreSQL with Entity Framework Core
- **Architecture**: Clean Architecture with CQRS pattern
- **API Documentation**: Swagger/OpenAPI
- **Containerization**: Docker

## Getting Started

### Prerequisites

- .NET Core SDK 3.0+
- PostgreSQL 11+
- Docker (optional)

### Running Locally

```bash
# Clone the repository
git clone https://github.com/yourusername/minicms.git
cd minicms

# Restore dependencies
dotnet restore

# Update database
dotnet ef database update -p src/MiniCMS.Infrastructure -s src/MiniCMS.Api

# Run the application
dotnet run -p src/MiniCMS.Api
```

### Using Docker

```bash
docker-compose up -d
```

The API will be available at `http://localhost:5000`

## API Documentation

Once running, access Swagger UI at: `http://localhost:5000/swagger`

### Quick Examples

```bash
# Create an app
POST /api/apps
{
  "name": "my-blog",
  "displayName": "My Blog"
}

# Create a schema
POST /api/apps/my-blog/schemas
{
  "name": "post",
  "fields": [
    { "name": "title", "type": "String", "required": true },
    { "name": "content", "type": "String" },
    { "name": "publishedAt", "type": "DateTime" }
  ]
}

# Create content
POST /api/apps/my-blog/post
{
  "data": {
    "title": "Hello World",
    "content": "My first post!",
    "publishedAt": "2019-06-15T10:00:00Z"
  }
}
```

## Project Structure

```
MiniCMS/
├── src/
│   ├── MiniCMS.Api/           # Web API controllers
│   ├── MiniCMS.Application/   # Business logic, CQRS
│   ├── MiniCMS.Domain/        # Core entities
│   └── MiniCMS.Infrastructure/ # Data access
└── tests/
    └── MiniCMS.Tests/         # Unit & integration tests
```

## Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Author

**Thanh Vu** - [thanhauco@gmail.com](mailto:thanhauco@gmail.com)
