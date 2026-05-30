import React from 'react';
import { useAuth } from '../context/AuthContext';
import './Dashboard.css';

export const Dashboard: React.FC = () => {
  const { user } = useAuth();

  return (
    <div className="dashboard-container">
      <div className="dashboard-box">
        <h1>Welcome, {user?.userName}!</h1>
        <div className="dashboard-info">
          <div className="info-item">
            <label>Email:</label>
            <span>{user?.email}</span>
          </div>
          <div className="info-item">
            <label>Role:</label>
            <span>{user?.role}</span>
          </div>
        </div>
        <p className="dashboard-message">
          {user?.role === 'Admin'
            ? 'You have full access to manage employees.'
            : 'You can view employee information.'}
        </p>
      </div>
    </div>
  );
};