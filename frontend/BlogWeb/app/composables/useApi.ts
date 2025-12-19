export const useApi = () => {
  const authStore = useAuthStore();
  // const config = useRuntimeConfig();
  const api = $fetch.create({
    baseURL: '/api/v1',
    onRequest({ options }) {
      const token = authStore.accessToken;
      if (token) {
        options.headers = new Headers(options.headers);
        options.headers.set('Authorization', `Bearer ${token}`);
      }
    },
    onResponseError({ response }) {
      const toast = useToast();
      const message = response._data?.message || response._data?.error || 'Something went wrong';
      toast.add({
        title: 'Error',
        description: message,
        color: 'error',
      });
    },
  });
  return <T>(url: string, options?: any): Promise<T> => {
    return api<T>(url, options);
  };
};
