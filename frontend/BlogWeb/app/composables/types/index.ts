export interface ApiResponse<T> {
  data: T;
  message?: string;
  error?: string;
  success: boolean;
}

export interface LoginResponse {
  token: string;
}
