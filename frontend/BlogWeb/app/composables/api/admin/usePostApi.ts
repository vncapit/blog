import type { ApiResponse, ListPostQuery, ListPostResponse, Post, UploadResponse } from '~/types';

export const usePostApi = () => {
  const { $api } = useNuxtApp();
  const list = async (query: ListPostQuery) => {
    const res = await $api<ApiResponse<ListPostResponse>>('Post/list', {
      method: 'GET',
      params: query,
    });
    return res.data;
  };

  const add = async (post: Omit<Post, 'id'>) => {
    const res = await $api<ApiResponse<ListPostResponse>>('Post/add', {
      method: 'POST',
      body: post,
    });
    return res.data;
  };

  const update = async (post: Post) => {
    const res = await $api<ApiResponse<ListPostResponse>>('Post/update', {
      method: 'PUT',
      body: post,
    });
    return res.data;
  };

  const del = async (id: number) => {
    const res = await $api<ApiResponse<ListPostResponse>>('Post/delete', {
      method: 'DELETE',
      body: { id },
    });
    return res.data;
  };

  const upload = async (image: File) => {
    const formData = new FormData();
    formData.append('file', image);
    const res = await $api<ApiResponse<UploadResponse>>('Post/upload', {
      method: 'POST',
      body: formData,
    });
    return res.data;
  };

  return { list, add, update, del, upload };
};
