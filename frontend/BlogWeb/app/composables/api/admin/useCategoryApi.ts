import type { ApiResponse, Category } from '~/types';

export const useCategoryApi = async () => {
  const { $api } = useNuxtApp();
  const res = await $api<ApiResponse<Category[]>>('Category/list', {
    method: 'GET',
  });
  return res.data;
};
