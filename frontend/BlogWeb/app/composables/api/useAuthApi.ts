import type { ApiResponse, LoginResponse } from '../types';

export const useAuthApi = async (data: { username: string; password: string }) => {
  const api = useApi();
  const res = await api<ApiResponse<LoginResponse>>('Auth/login', {
    method: 'POST',
    body: data,
  });
  return res.data;
};
