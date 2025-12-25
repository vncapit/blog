import { defineStore } from 'pinia';
import { useAuthApi } from '~/composables/api/admin/useAuthApi';
import type { ApiResponse, AuthResponse } from '~/types';
export const useAuthStore = defineStore('auth', {
  state: () => ({
    isAuthenticated: false,
    user: null as null | { id: number; name: string; email: string },
    accessToken: '' as string,
  }),
  actions: {
    async login(username: string, password: string) {
      const res = await useAuthApi({ username, password });
      this.accessToken = res.token;
      this.isAuthenticated = true;
      await navigateTo('/admin');
    },
    logout() {
      this.isAuthenticated = false;
      this.user = null;
      navigateTo('/admin/login');
    },
    async refreshAccessToken() {
      const res = await $fetch<ApiResponse<AuthResponse>>('/api/v1/Auth/refreshToken', {
        method: 'POST',
      });
      this.accessToken = res.data.token;
    },
  },
});
