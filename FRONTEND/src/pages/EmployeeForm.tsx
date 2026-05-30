import React, { useState, useEffect } from 'react';
import { useNavigate, useParams, useLocation } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import { Employee, EmployeeCreateDto, EmployeeUpdateDto } from '../types';
import apiService from '../services/api';
import './EmployeeForm.css';

export const EmployeeForm: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const location = useLocation();
  const navigate = useNavigate();
  const { user } = useAuth();

  const [formData, setFormData] = useState<EmployeeCreateDto>({
    userName: '',
    email: '',
    role: 'User'
  });
  const [loading, setLoading] = useState(!!id);
  const [error, setError] = useState<string | null>(null);
  const [submitting, setSubmitting] = useState(false);

  const isEditMode = !!id;
  const employee = location.state?.employee as Employee | undefined;

  useEffect(() => {
    if (isEditMode && employee) {
      setFormData({
        userName: employee.userName,
        email: employee.email,
        role: employee.role
      });
      setLoading(false);
    } else if (isEditMode && !employee) {
      fetchEmployee();
    }
  }, [id, isEditMode, employee]);

  const fetchEmployee = async () => {
    try {
      if (!id) return;
      const data = await apiService.getEmployee(parseInt(id));
      setFormData({
        userName: data.userName,
        email: data.email,
        role: data.role
      });
    } catch (err: any) {
      setError(err.response?.data?.message || 'Failed to load employee');
    } finally {
      setLoading(false);
    }
  };

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
    const { name, value } = e.target;
    setFormData((prev) => ({
      ...prev,
      [name]: value
    }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError(null);
    setSubmitting(true);

    try {
      if (isEditMode && id) {
        await apiService.updateEmployee(parseInt(id), formData as EmployeeUpdateDto);
      } else {
        await apiService.createEmployee(formData);
      }
      navigate('/employees');
    } catch (err: any) {
      setError(err.response?.data?.message || 'Failed to save employee');
    } finally {
      setSubmitting(false);
    }
  };

  if (!user || user.role !== 'Admin') {
    return <div className="form-container"><p>Access denied. Only admins can manage employees.</p></div>;
  }

  if (loading) {
    return <div className="form-container"><p>Loading...</p></div>;
  }

  return (
    <div className="form-container">
      <div className="form-box">
        <h1>{isEditMode ? 'Edit Employee' : 'Create Employee'}</h1>
        {error && <div className="error-message">{error}</div>}

        <form onSubmit={handleSubmit}>
          <div className="form-group">
            <label htmlFor="userName">Username</label>
            <input
              id="userName"
              type="text"
              name="userName"
              value={formData.userName}
              onChange={handleChange}
              required
              placeholder="Enter username"
            />
          </div>

          <div className="form-group">
            <label htmlFor="email">Email</label>
            <input
              id="email"
              type="email"
              name="email"
              value={formData.email}
              onChange={handleChange}
              required
              placeholder="Enter email"
            />
          </div>

          <div className="form-group">
            <label htmlFor="role">Role</label>
            <select
              id="role"
              name="role"
              value={formData.role}
              onChange={handleChange}
              required
            >
              <option value="User">User</option>
              <option value="Admin">Admin</option>
            </select>
          </div>

          <div className="form-actions">
            <button type="submit" disabled={submitting} className="btn-submit">
              {submitting ? 'Saving...' : isEditMode ? 'Update' : 'Create'}
            </button>
            <button
              type="button"
              onClick={() => navigate('/employees')}
              className="btn-cancel"
            >
              Cancel
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};