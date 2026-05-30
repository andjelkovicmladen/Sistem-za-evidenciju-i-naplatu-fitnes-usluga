# WebAPI Quick Start Guide

## Prerequisites
- .NET 8.0 SDK installed
- Visual Studio 2022/2026 (or Visual Studio Code)
- SQL Server LocalDB running
- Existing database "DB" with tables created

## 1. Opening the Project

### Visual Studio
1. Open the solution: `Sistem-za-evidenciju-i-naplatu-fitnes-usluga.sln`
2. The WebAPI project is already added to the solution
3. Build the solution: `Ctrl+Shift+B`

### Command Line
```bash
cd "Sistem-za-evidenciju-i-naplatu-fitnes-usluga-master"
dotnet build
```

## 2. Running the WebAPI

### Option 1: Visual Studio
1. Right-click "WebAPI" project → "Set as Startup Project"
2. Press `F5` to run with debugging
3. Swagger UI opens automatically at `https://localhost:5001/swagger`

### Option 2: Command Line
```bash
cd WebAPI
dotnet run
```

### Option 3: Visual Studio - Run Multiple Projects
1. Solution → Properties → Startup Project
2. Select "Multiple startup projects"
3. Set Action to "Start" for:
   - Server (WinForms)
   - WebAPI
4. Click OK, press F5

## 3. First-Time API Testing

### Step 1: Login
1. Navigate to Swagger UI: `http://localhost:5000/swagger`
2. Find `POST /api/auth/login` endpoint
3. Click "Try it out"
4. Enter admin credentials:
```json
{
  "email": "admin@fitnes.rs",
  "password": "admin123"
}
```
5. Click "Execute"
6. Copy the `token` value from response

### Step 2: Authorize in Swagger
1. Click blue "Authorize" button at top right
2. In the dialog, enter: `Bearer <paste-your-token-here>`
3. Click "Authorize"
4. Click "Close"

### Step 3: Test Protected Endpoints
1. Expand `GET /api/clanovi`
2. Click "Try it out"
3. Click "Execute"
4. View the list of all members

## 4. API Endpoints Reference

### Authentication
```
POST /api/auth/login
{
  "email": "admin@fitnes.rs",
  "password": "admin123"
}
```

### Members (Members - requires JWT)
```
GET    /api/clanovi              → Get all members
GET    /api/clanovi/1            → Get member with ID=1
POST   /api/clanovi              → Create new member
PUT    /api/clanovi/1            → Update member with ID=1
DELETE /api/clanovi/1            → Delete member with ID=1
POST   /api/clanovi/search       → Search members
```

### Invoices (requires JWT)
```
GET    /api/racuni               → Get all invoices
GET    /api/racuni/1             → Get invoice with ID=1
GET    /api/racuni/1/stavke      → Get invoice with line items
POST   /api/racuni               → Create new invoice
PUT    /api/racuni/1             → Update invoice with ID=1
POST   /api/racuni/search        → Search invoices
```

### Training Sessions (requires JWT)
```
POST   /api/termini              → Create training session
```

## 5. Running All Three Systems Together

The solution supports running all systems simultaneously:

### System 1: TCP Socket Server
- Located in `Server` project
- Listens on **port 9999**
- Handles WinForms client requests
- Windows Forms UI with server logs

### System 2: WinForms Client
- Located in `KorisnickiInterfejs` project
- Connects to TCP Server on port 9999
- Desktop GUI application
- Original functionality preserved

### System 3: WebAPI
- Located in `WebAPI` project
- Listens on **ports 5000 (HTTP) & 5001 (HTTPS)**
- REST API for web/mobile clients
- Swagger documentation

### To Run All Three:
1. Visual Studio → Solution Properties
2. Startup Project: "Multiple startup projects"
3. Set Action for all three to "Start":
   - Server
   - KorisnickiInterfejs
   - WebAPI
4. Click OK
5. Press F5

All three will start and can access the same database simultaneously.

## 6. Configuration

### Connection String
Edit `WebAPI/appsettings.json`:
```json
"ConnectionStrings": {
  "DefaultConnection": "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=DB;Integrated Security=True"
}
```

### JWT Secret (IMPORTANT)
**In Development**: Current secret is fine for testing

**In Production**: Change in `appsettings.json`:
```json
"Jwt": {
  "SecretKey": "your-very-long-secret-key-min-32-characters-long",
  "Issuer": "YourIssuer",
  "Audience": "YourAudience"
}
```

### CORS Configuration
Edit `Program.cs` to restrict origins:
```csharp
policy.WithOrigins("http://localhost:3000", "http://localhost:4200")
      .AllowAnyMethod()
      .AllowAnyHeader();
```

## 7. Common Commands

### Build the solution
```bash
dotnet build
```

### Build only WebAPI
```bash
cd WebAPI
dotnet build
```

### Run tests
```bash
dotnet test
```

### Clean build
```bash
dotnet clean
dotnet build
```

### Publish for production
```bash
cd WebAPI
dotnet publish -c Release
```

## 8. Troubleshooting

### "Connection timeout to database"
- Ensure SQL Server LocalDB is running
- Check connection string in appsettings.json
- Verify database "DB" exists

### "JWT token invalid"
- Token may have expired (24 hours)
- Login again to get new token
- Check Jwt:SecretKey is same in appsettings.json

### "CORS errors from frontend"
- Frontend likely on different port (3000, 4200, 8080)
- Update CORS policy in Program.cs
- Add frontend URLs to WithOrigins()

### "Port already in use"
- WebAPI default ports: 5000 (HTTP), 5001 (HTTPS)
- Change in launchSettings.json:
  ```json
  "applicationUrl": "https://localhost:5002;http://localhost:5001"
  ```

### "Authorization header not working"
- Format must be: `Authorization: Bearer <token>`
- Not just: `Authorization: <token>`
- In Swagger, enter: `Bearer <token-here>`

## 9. Development Tips

### Debug API Calls
1. Add breakpoint in controller or service
2. Run with `dotnet run` or F5
3. Call endpoint from Swagger or curl
4. Debugger will break at breakpoint

### View Logs
Open Package Manager Console:
```powershell
Get-Content "bin\Debug\net8.0\WebAPI.log"
```

### Test with cURL
```bash
# Login
curl -X POST https://localhost:5001/api/auth/login ^
  -H "Content-Type: application/json" ^
  -d "{\"email\":\"admin@fitnes.rs\",\"password\":\"admin123\"}"

# Get members (with token)
curl -X GET https://localhost:5001/api/clanovi ^
  -H "Authorization: Bearer <token>"
```

### Test with Postman
1. Import from Swagger: https://localhost:5001/swagger/v1/swagger.json
2. Create Environment with variable `token`
3. In login response, set `token` in Tests tab:
   ```javascript
   pm.environment.set("token", pm.response.json().token);
   ```
4. Use `{{token}}` in Authorization header for other requests

## 10. Next Steps

After confirming the API works:

1. **Build Frontend**: Create React/Angular app
2. **Test Integration**: Call API from frontend
3. **Add More Endpoints**: If needed for business logic
4. **Implement Caching**: For better performance
5. **Add Rate Limiting**: Prevent abuse
6. **Add Logging**: For production monitoring
7. **Deploy to Azure**: For public accessibility

---

## Support

For issues or questions:
- Check API documentation in Swagger UI
- Review README.md in WebAPI folder
- Check WEBAPI_IMPLEMENTATION_SUMMARY.md for complete details
- Review Program.cs for middleware configuration
- Check services in WebAPI/Services/ folder

Happy coding! 🚀
