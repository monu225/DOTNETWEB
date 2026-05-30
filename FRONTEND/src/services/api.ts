import axios, { AxiosInstance, AxiosError } from 'axios';
import { AuthResponse, Employee, EmployeeCreateDto, EmployeeUpdateDto, LoginRequest } from '../types';

const API_BASE_URL = process.env.REACT_APP_API_URL || 'http://localhost:5000/api';

class ApiService {
  private api: AxiosInstance;

  constructor() {
    this.api = axios.create({
      baseURL: API_BASE_URL,
      headers: {
        'Content-Type': 'application/json'
      }
    });

    // Add request interceptor to include JWT token
    this.api.interceptors.request.use((config) => {
      const token = localStorage.getItem('accessToken');
      if (token) {
        config.headers.Authorization = `Bearer ${token}`;
      }
      return config;
    });

    // Add response interceptor for error handling
    this.api.interceptors.response.use(
      (response) => response,
      (error: AxiosError) => {
        if (error.response?.status === 401) {
          localStorage.removeItem('accessToken');
          localStorage.removeItem('user');
          window.location.href = '/login';
        }
        return Promise.reject(error);
      }
    );
  }

  // Auth endpoints
  async login(email: string, password: string): Promise<AuthResponse> {
    const { data } = await this.api.post<AuthResponse>('/auth/login', {
      email,
      password
    } as LoginRequest);
    return data;
  }

  async logout(): Promise<void> {
    await this.api.post('/auth/logout');
  }

  // Employee endpoints
  async getEmployees(): Promise<Employee[]> {
    const { data } = await this.api.get<Employee[]>('/employees');
    return data;
  }

  async getEmployee(id: number): Promise<Employee> {
    const { data } = await this.api.get<Employee>(`/employees/${id}`);
    return data;
  }

  async createEmployee(dto: EmployeeCreateDto): Promise<Employee> {
    const { data } = await this.api.post<Employee>('/employees', dto);
    return data;
  }

  async updateEmployee(id: number, dto: EmployeeUpdateDto): Promise<void> {
    await this.api.put(`/employees/${id}`, dto);
  }

  async deleteEmployee(id: number): Promise<void> {
    await this.api.delete(`/employees/${id}`);
  }
}

export default new ApiService();