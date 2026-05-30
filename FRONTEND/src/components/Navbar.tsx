import React from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import './Navbar.css';

export const Navbar: React.FC = () => {
  const { user, isAuthenticated, logout } = useAuth();
  const navigate = useNavigate();

  const handleLogout = async () => {
    try {
      await logout();
      navigate('/login');
    } catch (err) {
      console.error('Logout failed:', err);
    }
  };

  return (
    <nav className="navbar">
      <div className="navbar-container">
        <Link to="/" className="navbar-logo">Employee Manager</Link>
        <div className="navbar-menu">
          {isAuthenticated ? (
            <>
              <span className="navbar-user">Welcome, {user?.userName}</span>
              <Link to="/employees" className="navbar-link">Employees</Link>
              {user?.role === 'Admin' && (
                <Link to="/employees/create" className="navbar-link">Create Employee</Link>
              )}
              <button onClick={handleLogout} className="navbar-logout-btn">Logout</button>
            </>
          ) : (
            <Link to="/login" className="navbar-link">Login</Link>
          )}
        </div>
      </div>
    </nav>
  );
};