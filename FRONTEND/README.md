# FRONTEND - Employee Management System

Modern React + TypeScript frontend for managing employees with JWT authentication.

## 🚀 Quick Start

### Installation

```bash
# Navigate to the frontend directory
cd FRONTEND

# Install dependencies
npm install

# Copy environment template
cp .env.example .env
```

### Development

```bash
# Start the development server
npm start
```

The app will open at `http://localhost:3000`

### Build

```bash
# Create optimized production build
npm run build

# Run tests
npm test
```

## 🔌 API Configuration

The frontend communicates with the backend API. Configure the endpoint in `.env`:

```env
REACT_APP_API_URL=http://localhost:5000/api
```

## 📋 Features

### Authentication
- User login with email/password
- JWT token management
- Automatic token refresh
- Logout with server-side token blacklist

### Employee Management
- View all employees
- Search and filter employees
- Create new employees (Admin only)
- Edit employee details (Admin only)
- Delete employees (Admin only)

### Access Control
- Role-based UI rendering
- Protected routes
- Admin-only features

## 🏗️ Project Structure

```
src/
├── components/           # Reusable React components
│   ├── Navbar.tsx       # Navigation component
│   ├── Navbar.css       # Navigation styles
│   └── ProtectedRoute.tsx # Route protection
│
├── pages/               # Page components
│   ├── Login.tsx        # Login page
│   ├── Dashboard.tsx    # Dashboard page
│   ├── Employees.tsx    # Employee list
│   ├── EmployeeForm.tsx # Create/Edit form
│   └── *.css           # Page-specific styles
│
├── services/            # API integration
│   └── api.ts          # Axios instance and API calls
│
├── context/             # React context
│   └── AuthContext.tsx  # Authentication state management
│
├── types/               # TypeScript interfaces
│   └── index.ts        # API and component types
│
├── App.tsx             # Main app component
├── index.tsx           # React DOM entry
└── index.css           # Global styles
```

## 🔐 Authentication Flow

1. User enters credentials on login page
2. API validates and returns JWT token
3. Token stored in `localStorage`
4. Axios interceptor adds token to all requests
5. On 401 response, token is cleared and user redirected to login
6. Logout blacklists token server-side

## 🎨 UI Components

### Navbar
- Logo and branding
- Navigation links
- User profile display
- Logout button

### Login Form
- Email input
- Password input
- Form validation
- Error messages

### Dashboard
- Welcome message
- User information
- Role-based content

### Employee List
- Table with employee data
- Status indicator (Active/Inactive)
- Edit/Delete buttons (Admin only)
- Create employee button (Admin only)

### Employee Form
- Create new employee
- Edit existing employee
- Username input
- Email input
- Role selection

## 📡 API Endpoints Used

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| POST | /auth/login | No | User login |
| POST | /auth/logout | Yes | User logout |
| GET | /employees | Yes | Get all employees |
| GET | /employees/:id | Yes | Get single employee |
| POST | /employees | Admin | Create employee |
| PUT | /employees/:id | Admin | Update employee |
| DELETE | /employees/:id | Admin | Delete employee |

## 🛠️ Development Tools

### Required
- Node.js 16+ ([download](https://nodejs.org/))
- npm 7+ (comes with Node.js)

### Recommended VS Code Extensions
- ES7+ React/Redux/React-Native snippets
- Prettier - Code formatter
- ESLint
- REST Client

## 📦 Dependencies

- **react** - UI framework
- **react-dom** - React DOM rendering
- **react-router-dom** - Client-side routing
- **axios** - HTTP client
- **typescript** - Type safety

## 🧪 Testing

Run tests in watch mode:
```bash
npm test
```

Run tests once:
```bash
npm test -- --watchAll=false
```

## 🔧 Troubleshooting

### "Cannot connect to API"
- Ensure backend is running on `http://localhost:5000`
- Check `REACT_APP_API_URL` in `.env`
- Verify backend CORS settings

### "401 Unauthorized"
- Token may have expired
- Clear localStorage and login again
- Check backend JWT configuration

### "Module not found"
- Run `npm install` to ensure all dependencies are installed
- Clear `node_modules` and reinstall if issues persist

## 🚀 Production Deployment

### Build the app
```bash
npm run build
```

### Environment variables for production
```env
REACT_APP_API_URL=https://your-api-domain.com/api
REACT_APP_ENV=production
```

### Deployment options
- Vercel
- Netlify
- AWS S3 + CloudFront
- GitHub Pages
- Azure Static Web Apps

## 📝 Git Workflow

```bash
# Create feature branch
git checkout -b feature/your-feature

# Make changes and commit
git add .
git commit -m "Add new feature"

# Push to remote
git push origin feature/your-feature

# Create pull request on GitHub
```

## 📚 Additional Resources

- [React Documentation](https://react.dev/)
- [TypeScript Handbook](https://www.typescriptlang.org/docs/)
- [React Router Guide](https://reactrouter.com/en/main)
- [Axios Documentation](https://axios-http.com/)

## 📄 License

MIT
