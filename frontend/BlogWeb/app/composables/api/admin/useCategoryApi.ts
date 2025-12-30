import type { ApiResponse, Category } from '~/types';

export const useCategoryApi = () => {
  const { $api } = useNuxtApp();
  const list = async () => {
    const res = await $api<ApiResponse<Category[]>>('Category/list', {
      method: 'GET',
    });
    return res.data;
  };

  const add = async (category: Omit<Category, 'id'>) => {
    const res = await $api<ApiResponse<Category[]>>('Category/add', {
      method: 'POST',
      body: category,
    });
    return res.data;
  };

  const update = async (category: Category) => {
    const res = await $api<ApiResponse<Category[]>>('Category/update', {
      method: 'PUT',
      body: category,
    });
    return res.data;
  };

  const del = async (id: number) => {
    const res = await $api<ApiResponse<Category[]>>('Category/delete', {
      method: 'DELETE',
      body: { id },
    });
    return res.data;
  };

  return { list, add, update, del };
};
