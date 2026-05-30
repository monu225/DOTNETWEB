# Employee Management System - Frontend

React + TypeScript frontend for the Employee Management System API.

## Features

- User authentication with JWT tokens
- Employee CRUD operations
- Role-based access control (Admin/User)
- Protected routes
- API error handling
- Responsive UI

## Prerequisites

- Node.js 16+
- npm or yarn

## Installation

```bash
cd FRONTEND
npm install
```

## Configuration

Create a `.env` file in the FRONTEND folder (copy from `.env.example`):

```bash
cp .env.example .env
```

Update the `REACT_APP_API_URL` if your API server is running on a different port.

## Running the Application

```bash
# Start development server
npm start

# Build for production
npm run build

# Run tests
npm test
```

The application will start on `http://localhost:3000`.

## API Configuration

The frontend expects the backend API to be running on `http://localhost:5000` by default.
You can change this by setting the `REACT_APP_API_URL` environment variable.

## Project Structure

```
FRONTEND/
├── src/
│   ├── components/        # Reusable components
│   │   ├── Navbar.tsx
│   │   ├── Navbar.css
│   │   └── ProtectedRoute.tsx
│   ├── pages/            # Page components
│   │   ├── Login.tsx
│   │   ├── Dashboard.tsx
│   │   ├── Employees.tsx
│   │   └── EmployeeForm.tsx
│   ├── services/         # API service layer
│   │   └── api.ts
│   ├── context/          # React context
│   │   └── AuthContext.tsx
│   ├── types/            # TypeScript interfaces
│   │   └── index.ts
│   ├── App.tsx
│   ├── index.tsx
│   └── index.css
├── public/
│   └── index.html
├── package.json
├── tsconfig.json
├── .env.example
└── README.md
```

## Authentication

The application uses JWT (JSON Web Tokens) for authentication. Tokens are stored in `localStorage`.

## User Roles

- **Admin**: Can create, read, update, and delete employees
- **User**: Can only read employee information

## License

MIT
