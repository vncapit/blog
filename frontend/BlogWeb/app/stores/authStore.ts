import { defineStore } from 'pinia';
import { useAuthApi } from '~/composables/api/useAuthApi';
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
    },
    logout() {
      this.isAuthenticated = false;
      this.user = null;
    },
  },
});
