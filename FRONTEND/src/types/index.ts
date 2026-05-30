export interface Employee {
  id: number;
  userName: string;
  email: string;
  role: string;
  isActive: boolean;
  createdAt: string;
  updatedAt: string;
}

export interface EmployeeCreateDto {
  userName: string;
  email: string;
  role: string;
}

export interface EmployeeUpdateDto {
  userName: string;
  email: string;
  role: string;
}

export interface AuthResponse {
  accessToken: string;
  expiresIn: number;
  user: {
    id: number;
    userName: string;
    email: string;
    role: string;
  };
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface RegisterRequest {
  userName: string;
  email: string;
  password: string;
  role: string;
}

export interface ApiResponse<T> {
  data?: T;
  message?: string;
  error?: string;
}