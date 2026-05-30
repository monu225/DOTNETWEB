# Full-Stack Setup Guide

Complete step-by-step guide to get the Employee Management System running locally.

## Prerequisites

- **.NET 10.0 SDK** - [Download](https://dotnet.microsoft.com/download/dotnet/10.0)
- **Node.js 16+** - [Download](https://nodejs.org/)
- **SQL Server Express or Local DB** - [Download](https://www.microsoft.com/sql-server/sql-server-downloads)
- **Git** - [Download](https://git-scm.com/)
- **Visual Studio Code** (Optional) - [Download](https://code.visualstudio.com/)

## Step 1: Backend Setup

### 1.1 Navigate to Backend Directory
```bash
cd /path/to/DOTNETWEB
```

### 1.2 Install Backend Dependencies
```bash
dotnet restore
```

### 1.3 Configure Database Connection

Edit `appsettings.json` and update the connection string to match your SQL Server instance:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost\\SQLEXPRESS01;Database=WebApiCrudDb;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

**Options for Server:**
- Local machine: `localhost\\SQLEXPRESS01`
- Named instance: `COMPUTERNAME\\INSTANCENAME`
- TCP/IP: `tcp:localhost,1433`

### 1.4 Run Migrations (If Using EF Core)

If migrations exist in the project:
```bash
dotnet ef database update
```

### 1.5 Configure JWT Settings

Verify JWT settings in `appsettings.json`:

```json
{
  "Jwt": {
    "Key": "ReplaceThisDevelopmentOnlyJwtSecretKeyWithAtLeast32Chars",
    "Issuer": "WEBAPI_CRUD",
    "Audience": "WEBAPI_CRUD_CLIENT",
    "ExpiresInMinutes": 60
  }
}
```

⚠️ **Important:** Change the `Key` to a secure value for production.

### 1.6 Start Backend Server

```bash
dotnet run
```

You should see output similar to:
```
info: Microsoft.AspNetCore.Hosting.Hosting
      Now listening on: http://localhost:5000
      Now listening on: https://localhost:5001
```

The API is ready when you see these messages. Leave this terminal open.

## Step 2: Frontend Setup

### 2.1 Open New Terminal and Navigate to Frontend

```bash
# In a new terminal window/tab
cd /path/to/DOTNETWEB/FRONTEND
```

### 2.2 Install Frontend Dependencies

```bash
npm install
```

This will install all required npm packages:
- React
- React Router
- Axios
- TypeScript
- And development dependencies

### 2.3 Configure Environment Variables

```bash
cp .env.example .env
```

Edit `.env` and set the API URL (usually no changes needed for local development):

```env
REACT_APP_API_URL=http://localhost:5000/api
REACT_APP_ENV=development
```

### 2.4 Start Frontend Development Server

```bash
npm start
```

The browser should automatically open at `http://localhost:3000`. If not, navigate there manually.

## Step 3: Verify Installation

### 3.1 Test Backend API

Open your browser or API client (Postman, Insomnia) and test:

```
GET http://localhost:5000/api/employees
```

Expected response (requires authentication):
```json
{
  "message": "Unauthorized"
}
```

### 3.2 Test Frontend

Navigate to `http://localhost:3000` in your browser. You should see:
- Login page at `/login`
- Navigation bar
- Clean, responsive interface

### 3.3 Create Test User

To create a test user, you have two options:

**Option A: Using the API (if register endpoint is available)**
```bash
POST http://localhost:5000/api/auth/register
Content-Type: application/json
Authorization: Bearer <admin_token>

{
  "userName": "testuser",
  "email": "test@example.com",
  "password": "TestPassword123!",
  "role": "User"
}
```

**Option B: Direct Database Entry**
1. Open SQL Server Management Studio
2. Connect to your server
3. Navigate to `WebApiCrudDb` database
4. Insert records into the Users/Employees table

Sample SQL:
```sql
INSERT INTO Users (UserName, Email, PasswordHash, Role, IsActive)
VALUES ('testadmin', 'admin@example.com', '<bcrypt_hash>', 'Admin', 1)
```

### 3.4 Login to Frontend

1. Go to `http://localhost:3000`
2. Enter credentials
3. Click Login
4. You should be redirected to Dashboard

## Troubleshooting

### Backend Issues

**"Port 5000 already in use"**
```bash
# Specify a different port
dotnet run --urls "http://localhost:5001"
# Then update .env in FRONTEND to use 5001
```

**"Connection timeout" / "Cannot connect to database"**
- Verify SQL Server is running
- Check connection string in `appsettings.json`
- Ensure database exists or create it manually

**"JWT configuration error"**
- Verify `Jwt` section exists in `appsettings.json`
- Ensure `Key` is at least 32 characters

### Frontend Issues

**"Cannot find module" after npm install**
```bash
# Clear cache and reinstall
rm -rf node_modules package-lock.json
npm install
```

**"Cannot connect to API"**
- Ensure backend is running on port 5000
- Check `REACT_APP_API_URL` in `.env`
- Clear browser cache (Ctrl+Shift+Delete)

**"npm install fails"**
```bash
# Use npm cache clean
npm cache clean --force
npm install
```

## Running in Production

### Build Backend
```bash
dotnet publish -c Release -o ./publish
cd publish
dotnet WEBAPI_CRUD.dll
```

### Build Frontend
```bash
cd FRONTEND
npm run build
# Contents of build/ folder ready to deploy
```

## Next Steps

1. **Explore the API**: Check `WEBAPI_CRUD.http` for request examples
2. **Create sample data**: Add test employees through the UI
3. **Test role-based access**: Try operations with different user roles
4. **Review code**: Study the implementation patterns used
5. **Customize**: Adapt to your specific requirements

## Common Commands Reference

### Backend
```bash
dotnet run                    # Run development server
dotnet build                  # Compile project
dotnet test                   # Run unit tests (if available)
dotnet publish -c Release     # Build for production
```

### Frontend
```bash
npm start                     # Run development server
npm run build                 # Create production build
npm test                      # Run test suite
npm install <package>         # Add new dependency
npm update                    # Update dependencies
```

### Git
```bash
git clone <repo>              # Clone repository
git checkout -b <branch>      # Create new branch
git add .                     # Stage all changes
git commit -m "message"       # Commit changes
git push origin <branch>      # Push to remote
```

## Documentation Links

- [.NET 10 Documentation](https://learn.microsoft.com/en-us/dotnet/)
- [ASP.NET Core Guide](https://learn.microsoft.com/en-us/aspnet/core/)
- [React Documentation](https://react.dev/)
- [TypeScript Handbook](https://www.typescriptlang.org/docs/)

## Need Help?

1. Check the individual README files:
   - `README.md` - Main project overview
   - `FRONTEND/README.md` - Frontend specific guide
   
2. Review the API structure in `Controllers/`
3. Check component implementation in `FRONTEND/src/components/`
4. Review services in `FRONTEND/src/services/`

## Security Notes

⚠️ **Before deploying to production:**
1. Change JWT secret key to a secure value
2. Enable HTTPS/SSL certificates
3. Set `Nullable = false` appropriately
4. Configure CORS for specific domains
5. Use environment-specific configuration files
6. Secure sensitive data with Azure Key Vault or similar
7. Implement rate limiting
8. Add request logging and monitoring

---

**Last Updated:** May 2026  
**Maintainer:** Monu Vishwakarma  
**License:** MIT
