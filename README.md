# API Validation Framework

A minimal ASP.NET Core Web API project demonstrating semantic and structural validation for REST API endpoints. This project includes middleware for semantic validation and controller-level validation using DTOs with auto-generated API documentation.

## Features

- **Semantic Validation Middleware**: Ensures business rules (e.g., `EndDate` must be after `StartDate`) before controller logic.
- **DTO-based Validation**: Uses data annotations for structural validation.
- **Swagger/OpenAPI**: Auto-generated API documentation.
- **Unit Tests**: xUnit-based tests for validation scenarios.

## Project Structure

```
ApiValidationFramework/
  Controllers/
    OrderController.cs
  Dtos/
    OrderDtos.cs
  middlewares/
    SemanticValidationMiddleware.cs
  models/
    Order.cs
  Program.cs
  appsettings.json
  ...
ApiValidationFramework.Tests/
  OrderControllerTests.cs
  ...
.github/
  workflows/
    dotnet-ci.yml
```

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

### Build and Run

```sh
dotnet build
dotnet run --project ApiValidationFramework/ApiValidationFramework.csproj
```

The API will be available at [http://localhost:5155](http://localhost:5155) (see `launchSettings.json`).

## API Usage

### Create Order

**POST** `/order`

**Request Body:**
```json
{
  "productName": "Nike AF1",
  "quantity": 10,
  "startDate": "2025-08-01",
  "endDate": "2025-08-10"
}
```

**Response:**
```json
{
  "message": "Order created",
  "order": {
    "id": "123e4567-e89b-12d3-a456-426614174000",
    "productName": "Nike AF1",
    "quantity": 10,
    "startDate": "2025-08-01T00:00:00Z",
    "endDate": "2025-08-10T00:00:00Z",
    "createdAt": "2025-01-27T10:30:00Z",
    "updatedAt": null
  }
}
```

### Get All Orders

**GET** `/orders`

**Response:**
```json
[
  {
    "id": "123e4567-e89b-12d3-a456-426614174000",
    "productName": "Nike AF1",
    "quantity": 10,
    "startDate": "2025-08-01T00:00:00Z",
    "endDate": "2025-08-10T00:00:00Z",
    "createdAt": "2025-01-27T10:30:00Z",
    "updatedAt": null
  }
]
```

### Get Order by ID

**GET** `/order/{id}`

**Response:**
```json
{
  "id": "123e4567-e89b-12d3-a456-426614174000",
  "productName": "Nike AF1",
  "quantity": 10,
  "startDate": "2025-08-01T00:00:00Z",
  "endDate": "2025-08-10T00:00:00Z",
  "createdAt": "2025-01-27T10:30:00Z",
  "updatedAt": null
}
```

### Update Order

**PUT** `/order/{id}`

**Request Body:**
```json
{
  "productName": "Adidas Superstar",
  "quantity": 5,
  "startDate": "2025-08-02",
  "endDate": "2025-08-12"
}
```

**Response:**
```json
{
  "message": "Order updated",
  "order": {
    "id": "123e4567-e89b-12d3-a456-426614174000",
    "productName": "Adidas Superstar",
    "quantity": 5,
    "startDate": "2025-08-02T00:00:00Z",
    "endDate": "2025-08-12T00:00:00Z",
    "createdAt": "2025-01-27T10:30:00Z",
    "updatedAt": "2025-01-27T11:00:00Z"
  }
}
```

### Delete Order

**DELETE** `/order/{id}`

**Response:**
```json
{
  "message": "Order deleted successfully"
}
```

**Validation Rules:**
- `productName`: Required, max 100 chars
- `quantity`: 1–1000
- `endDate` must be after `startDate`
- All validation rules apply to both Create and Update operations

## Swagger UI

Visit [http://localhost:5155/swagger](http://localhost:5155/swagger) for interactive API docs.

## Running Tests

```sh
dotnet test
```

## Continuous Integration

CI is configured via `.github/workflows/dotnet-ci.yml` to build, test, and generate Swagger docs on push and PRs to `main`.

## Key Files

- `Program.cs`: App entry, middleware, and controller setup.
- `OrderController.cs`: Order API endpoint.
- `SemanticValidationMiddleware.cs`: Business rule validation.
- `OrderControllerTests.cs`: Unit tests for order validation.
