import type { ApiResponse, AuthResponse } from '~/types';

export default defineNuxtPlugin(() => {
  const auth = useAuthStore();
  const toast = useToast();

  const api = $fetch.create({
    baseURL: '/api/v1',
    onRequest({ options }) {
      if (auth.accessToken) {
        options.headers = new Headers(options.headers);
        options.headers.set('Authorization', `Bearer ${auth.accessToken}`);
      }
    },
    async onResponseError(context) {
      const { response, options, request } = context;
      if (response.status !== 401) {
        toast.add({
          title: 'Error',
          description: response._data?.message ?? 'Request failed',
          color: 'error',
        });
        throw response;
      }
      try {
        await auth.refreshAccessToken();
        options.headers = new Headers(options.headers);
        options.headers.set('Authorization', `Bearer ${auth.accessToken}`);
        context.response = await $fetch(request, options as any);
      } catch (error) {
        auth.logout();
      }
    },
  });
  const apiWrapper = <T>(url: string, options?: any): Promise<T> => {
    return api<T>(url, options);
  };
  return { provide: { api: apiWrapper } };
});
