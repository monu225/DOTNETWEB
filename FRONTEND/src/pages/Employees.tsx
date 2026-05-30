import React, { useState, useEffect } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import { Employee } from '../types';
import apiService from '../services/api';
import './Employees.css';

export const Employees: React.FC = () => {
  const [employees, setEmployees] = useState<Employee[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [successMessage, setSuccessMessage] = useState<string | null>(null);
  const { user } = useAuth();
  const navigate = useNavigate();

  useEffect(() => {
    fetchEmployees();
  }, []);

  const fetchEmployees = async () => {
    setLoading(true);
    setError(null);
    try {
      const data = await apiService.getEmployees();
      setEmployees(data);
    } catch (err: any) {
      setError(err.response?.data?.message || 'Failed to load employees');
    } finally {
      setLoading(false);
    }
  };

  const handleDelete = async (id: number) => {
    if (!window.confirm('Are you sure you want to delete this employee?')) {
      return;
    }
    try {
      await apiService.deleteEmployee(id);
      setSuccessMessage('Employee deleted successfully');
      setTimeout(() => setSuccessMessage(null), 3000);
      fetchEmployees();
    } catch (err: any) {
      setError(err.response?.data?.message || 'Failed to delete employee');
    }
  };

  if (loading) {
    return <div className="employees-container"><p>Loading...</p></div>;
  }

  return (
    <div className="employees-container">
      <div className="employees-header">
        <h1>Employees</h1>
        {user?.role === 'Admin' && (
          <Link to="/employees/create" className="btn-create">+ Create Employee</Link>
        )}
      </div>

      {error && <div className="error-message">{error}</div>}
      {successMessage && <div className="success-message">{successMessage}</div>}

      {employees.length === 0 ? (
        <p className="no-data">No employees found.</p>
      ) : (
        <div className="employees-table-wrapper">
          <table className="employees-table">
            <thead>
              <tr>
                <th>ID</th>
                <th>Username</th>
                <th>Email</th>
                <th>Role</th>
                <th>Status</th>
                <th>Created</th>
                {user?.role === 'Admin' && <th>Actions</th>}
              </tr>
            </thead>
            <tbody>
              {employees.map((emp) => (
                <tr key={emp.id}>
                  <td>{emp.id}</td>
                  <td>{emp.userName}</td>
                  <td>{emp.email}</td>
                  <td>{emp.role}</td>
                  <td>
                    <span className={`status ${emp.isActive ? 'active' : 'inactive'}`}>
                      {emp.isActive ? 'Active' : 'Inactive'}
                    </span>
                  </td>
                  <td>{new Date(emp.createdAt).toLocaleDateString()}</td>
                  {user?.role === 'Admin' && (
                    <td className="actions">
                      <button
                        className="btn-edit"
                        onClick={() => navigate(`/employees/${emp.id}/edit`, { state: { employee: emp } })}
                      >
                        Edit
                      </button>
                      <button
                        className="btn-delete"
                        onClick={() => handleDelete(emp.id)}
                      >
                        Delete
                      </button>
                    </td>
                  )}
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}
    </div>
  );
};