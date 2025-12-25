import type { ApiResponse, AuthResponse } from '~/types';

export const useAuthApi = async (data: { username: string; password: string }) => {
  const { $api } = useNuxtApp();
  const res = await $api<ApiResponse<AuthResponse>>('Auth/login', {
    method: 'POST',
    body: data,
  });
  return res.data;
};
