# DOTNETWEB - Employee Management System

Complete full-stack solution with a .NET Core API backend and React TypeScript frontend for managing employees with JWT authentication and role-based access control.

## 🏗️ Project Structure

```
DOTNETWEB/
├── FRONTEND/              # React + TypeScript frontend
│   ├── src/
│   │   ├── components/    # Reusable components
│   │   ├── pages/         # Page components
│   │   ├── services/      # API service layer
│   │   ├── context/       # React context (Auth)
│   │   ├── types/         # TypeScript interfaces
│   │   ├── App.tsx
│   │   └── index.tsx
│   ├── public/
│   ├── package.json
│   ├── tsconfig.json
│   └── README.md
│
└── Backend API/           # .NET Core 10.0 Web API
    ├── Controllers/       # API endpoints
    ├── DTOs/             # Data Transfer Objects
    ├── Models/           # Database models
    ├── Services/         # Business logic
    ├── Repositories/     # Data access layer
    ├── Middleware/       # Custom middleware
    ├── Program.cs        # Application setup
    └── appsettings.json  # Configuration
```

## ✨ Features

### Backend API (.NET Core)
- ✅ RESTful API endpoints for employee management
- ✅ JWT authentication and authorization
- ✅ Role-based access control (Admin/User)
- ✅ Token blacklist for logout functionality
- ✅ SQL Server database integration
- ✅ Exception handling middleware
- ✅ Data validation with DTOs

### Frontend (React + TypeScript)
- ✅ User login/authentication
- ✅ Protected routes with JWT tokens
- ✅ Employee list view
- ✅ Create/Edit/Delete employees (Admin only)
- ✅ Dashboard with user info
- ✅ Role-based UI rendering
- ✅ Error handling and loading states
- ✅ Responsive design

## 🚀 Getting Started

### Prerequisites

- **.NET 10.0 SDK** (for backend)
- **Node.js 16+** and **npm** (for frontend)
- **SQL Server** or SQL Server Express
- **Git**

### Backend Setup

1. **Install dependencies:**
   ```bash
   dotnet restore
   ```

2. **Update database connection string** in `appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=localhost\\SQLEXPRESS01;Database=WebApiCrudDb;Trusted_Connection=True;TrustServerCertificate=True;"
     }
   }
   ```

3. **Run the API:**
   ```bash
   dotnet run
   ```
   The API will be available at `http://localhost:5000`

### Frontend Setup

1. **Navigate to the frontend folder:**
   ```bash
   cd FRONTEND
   ```

2. **Install dependencies:**
   ```bash
   npm install
   ```

3. **Create `.env` file:**
   ```bash
   cp .env.example .env
   ```
   Update `REACT_APP_API_URL` if needed (default: `http://localhost:5000/api`)

4. **Start the development server:**
   ```bash
   npm start
   ```
   The frontend will be available at `http://localhost:3000`

## 📚 API Endpoints

### Authentication
- `POST /api/auth/login` - Login with email and password
- `POST /api/auth/logout` - Logout (requires authentication)
- `POST /api/auth/register` - Register new user (Admin only)

### Employees
- `GET /api/employees` - Get all employees
- `GET /api/employees/{id}` - Get employee by ID
- `POST /api/employees` - Create employee (Admin only)
- `PUT /api/employees/{id}` - Update employee (Admin only)
- `DELETE /api/employees/{id}` - Delete employee (Admin only)

## 🔐 User Roles

### Admin
- Can create, read, update, and delete employees
- Can register new users
- Full access to all features

### User
- Can only view employee information
- Cannot modify employee data

## 💾 Default Credentials

Use these test credentials to login (if pre-populated in the database):

```
Email: admin@example.com
Password: [Password from DB]

Email: user@example.com
Password: [Password from DB]
```

**Note:** Passwords are hashed with bcrypt. First-time setup may require direct database entry.

## 🔧 Configuration

### Backend Configuration (appsettings.json)

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Your SQL Server connection string"
  },
  "Jwt": {
    "Key": "Your JWT signing key (min 32 chars)",
    "Issuer": "WEBAPI_CRUD",
    "Audience": "WEBAPI_CRUD_CLIENT",
    "ExpiresInMinutes": 60
  }
}
```

### Frontend Configuration (.env)

```
REACT_APP_API_URL=http://localhost:5000/api
REACT_APP_ENV=development
```

## 🧪 Testing

### API Testing
- Use Postman, Insomnia, or the included `WEBAPI_CRUD.http` file
- Import the `.http` file in VS Code with the REST Client extension

### Frontend Testing
```bash
cd FRONTEND
npm test
```

## 📦 Build for Production

### Backend
```bash
dotnet publish -c Release -o ./publish
```

### Frontend
```bash
cd FRONTEND
npm run build
```

## 🛠️ Technology Stack

### Backend
- .NET 10.0
- ASP.NET Core
- Entity Framework Core
- SQL Server
- JWT Authentication
- Bcrypt password hashing

### Frontend
- React 18.3+
- TypeScript 5.5+
- React Router DOM 6.25+
- Axios for HTTP client
- CSS3 (no external CSS framework)

## 📝 Project Notes

- JWT tokens are stored in browser's `localStorage`
- Tokens expire after the configured duration (default: 60 minutes)
- Logout functionality blacklists the token server-side
- Admin role is required for certain operations
- All API responses follow a consistent structure

## 🤝 Contributing

1. Create a feature branch
2. Make your changes
3. Test thoroughly
4. Commit with descriptive messages
5. Push to GitHub

## 📄 License

MIT
