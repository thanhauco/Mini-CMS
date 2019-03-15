# MiniCMS Implementation Plan

## Architecture

Clean Architecture with CQRS pattern:

```
┌─────────────────────────────────────────────────────────┐
│                     API Layer                            │
│              (Controllers, Middleware)                   │
├─────────────────────────────────────────────────────────┤
│                  Application Layer                       │
│            (Commands, Queries, Handlers)                 │
├─────────────────────────────────────────────────────────┤
│                    Domain Layer                          │
│              (Entities, Value Objects)                   │
├─────────────────────────────────────────────────────────┤
│                 Infrastructure Layer                     │
│            (EF Core, Repositories, Services)             │
└─────────────────────────────────────────────────────────┘
```

## Core Entities

- **App** - Multi-tenant container
- **Schema** - Dynamic content type definitions
- **Content** - Actual content items with JSON data
- **Asset** - Media files

## Field Types

- String, Number, Boolean, DateTime
- Assets, References, Array, Json

## API Endpoints

| Resource | Endpoints                           |
| -------- | ----------------------------------- |
| Apps     | `GET/POST /api/apps`                |
| Schemas  | `GET/POST /api/apps/{app}/schemas`  |
| Contents | `GET/POST /api/apps/{app}/{schema}` |
| Assets   | `GET/POST /api/apps/{app}/assets`   |

## Tech Stack

- ASP.NET Core 3.0
- Entity Framework Core 3.0
- PostgreSQL
- Docker
