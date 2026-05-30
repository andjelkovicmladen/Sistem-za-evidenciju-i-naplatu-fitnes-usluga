# WebAPI Project Documentation

## Overview
The WebAPI project is an ASP.NET Core 8.0 REST API for the Fitness Center Management system. It provides RESTful endpoints for managing members (članovi), invoices (računi), training sessions (termini), and authentication.

## Architecture

### Services Layer
Services encapsulate business logic and reuse the same broker pattern from the original Server/Operacije classes:

- **AuthService**: Handles administrator authentication and JWT token generation
- **ClanService**: Manages member CRUD operations and search functionality
- **RacunService**: Manages invoice CRUD operations with support for invoice line items (stavke)
- **TerminService**: Manages training session scheduling

### Controllers
RESTful API controllers handle HTTP requests:

- **AuthController**: POST /api/auth/login - Authenticates administrators and returns JWT tokens
- **ClanoviController**: CRUD operations for members
  - GET /api/clanovi - Get all members
  - GET /api/clanovi/{id} - Get member by ID
  - POST /api/clanovi - Create new member
  - PUT /api/clanovi/{id} - Update member
  - DELETE /api/clanovi/{id} - Delete member
  - POST /api/clanovi/search - Search members

- **RacuniController**: CRUD operations for invoices
  - GET /api/racuni - Get all invoices
  - GET /api/racuni/{id} - Get invoice by ID
  - GET /api/racuni/{id}/stavke - Get invoice with line items
  - POST /api/racuni - Create new invoice
  - PUT /api/racuni/{id} - Update invoice
  - POST /api/racuni/search - Search invoices

- **TerminiController**: Training session management
  - POST /api/termini - Create new training session

## Authentication & Authorization

### JWT Configuration
The API uses JWT (JSON Web Tokens) for authentication. Configure in `appsettings.json`:

```json
"Jwt": {
  "SecretKey": "your-super-secret-key-change-this-in-production-min-32-chars!",
  "Issuer": "FitnesCenterAPI",
  "Audience": "FitnesCenterClients"
}
```

### Login Flow
1. POST to `/api/auth/login` with email and password
2. Receive JWT token in response
3. Include token in Authorization header for protected endpoints: `Authorization: Bearer <token>`

### Token Claims
JWT tokens include:
- NameIdentifier: Administrator ID
- Email: Administrator email
- Name: Administrator full name

## Database Connection

The API reads the connection string from `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=DB;Integrated Security=True"
}
```

## CORS Configuration

The API is configured with CORS policy "AllowFrontend" to allow requests from any origin:

```csharp
options.AddPolicy("AllowFrontend", policy =>
{
    policy.AllowAnyOrigin()
          .AllowAnyMethod()
          .AllowAnyHeader();
});
```

This allows future React/Angular frontends to call the API without CORS restrictions.

## Running the API

### Development
```bash
dotnet run
```

The API will:
- Start on `https://localhost:5001` and `http://localhost:5000`
- Open Swagger UI automatically at `/swagger`

### Swagger/OpenAPI Documentation
Interactive API documentation is available at:
- `http://localhost:5000/swagger`
- `https://localhost:5001/swagger`

Test endpoints directly from the Swagger UI by:
1. Click "Authorize" button
2. Paste JWT token from login response
3. Try out endpoints

## Dependency Injection

All services are registered as Scoped lifetime in `Program.cs`:

```csharp
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IClanService, ClanService>();
builder.Services.AddScoped<IRacunService, RacunService>();
builder.Services.AddScoped<ITerminService, TerminService>();
```

## Data Models

Models are shared from the Domen project:
- **Administrator**: API administrator/user
- **Clan**: Fitness center member
- **Racun**: Invoice
- **StavkaRacuna**: Invoice line item
- **TerminTreninga**: Training session
- **FitnesUsluga**: Fitness service/offering
- **TipClanarine**: Membership type

## Integration with Existing System

- ✅ Reuses Broker and BrokerBP patterns from original Server
- ✅ Accesses same SQL Server LocalDB database
- ✅ Shares entity models from Domen project
- ✅ Shares DTOs from Zajednicki project
- ✅ Keeps original TCP Socket Server fully operational
- ✅ Keeps original WinForms client fully functional

## Error Handling

All endpoints implement consistent error handling:
- 400 Bad Request: Invalid input data
- 401 Unauthorized: Missing/invalid JWT token
- 404 Not Found: Resource not found
- 500 Internal Server Error: Server error

Responses include descriptive error messages in JSON format:
```json
{
  "message": "Error description"
}
```

## Example Usage

### Login
```bash
curl -X POST https://localhost:5001/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"email":"admin@fitnes.rs","password":"admin123"}'
```

Response:
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "admin": {
    "idAdministrator": 1,
    "imePrezime": "Petar Petrovic",
    "email": "admin@fitnes.rs"
  }
}
```

### Get All Members (with JWT)
```bash
curl -X GET https://localhost:5001/api/clanovi \
  -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
```

## Future Enhancements

1. Implement refresh tokens for longer session persistence
2. Add role-based authorization (Admin, Staff, Member)
3. Implement audit logging for all modifications
4. Add pagination for large result sets
5. Implement request validation using FluentValidation
6. Add API versioning strategy
7. Implement caching layer for frequently accessed data
8. Add comprehensive unit and integration tests

## Notes

- The WebAPI does not interfere with the existing TCP Socket Server on port 9999
- The WebAPI does not interfere with the existing WinForms client
- Both systems can run simultaneously and access the same database
- JWT secret key should be changed in production environments
- Consider implementing more granular CORS policies for production
