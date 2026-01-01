<template>
  <div class="flex flex-col flex-1">
    <h1 class="text-2xl font-bold mb-4">Post Management</h1>
    <div class="my-2 w-full">
      <UInput
        v-model="query.titleContains"
        placeholder="Search by title"
        class="w-64"
        @keyup.enter="refreshPosts"
      />
      <USelect
        v-model="query.categoryId"
        :items="categories"
        clearable
        placeholder="Filter by category"
        class="w-48 ml-2"
      />
      <UButton class="ml-2" @click="toSearch"> Search </UButton>
      <UButton class="ml-2" color="warning" @click="toReset"> Reset </UButton>
    </div>
    <div class="mt-2 mb-2">
      <UButton @click="addPost" color="primary"> Add Post </UButton>
    </div>
    <UTable :data="posts || []" :columns="columns" class="flex-1 w-full" />
    <div class="flex justify-end border-t border-default pt-4 px-4">
      <UPagination
        :page="query.pageNumber"
        :items-per-page="query.pageSize"
        :total="totalCount"
        @update:page="
          (pageChanged) => {
            query.pageNumber = pageChanged;
            refreshPosts();
          }
        "
      />
    </div>
    <UModal v-model:open="openForm" :dismissible="false" :title="formTitle">
      <template #body>
        <UForm ref="formRef" :schema="schema" :state="form" class="space-y-4">
          <UFormField label="Post" name="name" required>
            <UInput v-model="form.name" class="w-full" />
          </UFormField>
        </UForm>
      </template>
      <template #footer>
        <div class="w-full flex justify-end gap-2">
          <UButton variant="text" @click="openForm = false">Cancel</UButton>
          <UButton @click="submitForm">Save</UButton>
        </div>
      </template>
    </UModal>
  </div>
</template>

<script setup lang="ts">
  import { h, resolveComponent } from 'vue';
  import type { TableColumn } from '@nuxt/ui';
  import { usePostApi } from '~/composables/api/admin/usePostApi';
  import { useCategoryApi } from '~/composables/api/admin/useCategoryApi';
  import { ref, reactive, onMounted } from 'vue';
  import * as z from 'zod';
  import type { Category, ListPostQuery, Post } from '~/types';
  import { Status } from '~/types';
  import type { SelectItem } from '@nuxt/ui';

  definePageMeta({
    layout: 'admin',
  });

  const toast = useToast();
  const postApi = usePostApi();

  const UButton = resolveComponent('UButton');
  const UPopover = resolveComponent('UPopover');

  const posts = ref<Post[]>([]);

  const formRef = useTemplateRef('formRef');
  const schema = z.object({
    name: z.string().min(1, 'Post name is required'),
  });

  const defaultForm = { id: undefined as number | undefined, name: '' };
  const openForm = ref(false);
  const formTitle = ref('Add Post');

  const form = reactive({ ...defaultForm });

  onMounted(async () => {
    refreshPosts();
    getCategories();
  });

  const categories = ref<SelectItem[]>([]);
  const getCategories = async () => {
    const res = await useCategoryApi().list();
    categories.value = res.map((cat: Category) => ({
      label: cat.name,
      value: cat.id,
    }));
  };

  const query = reactive<ListPostQuery>({
    titleContains: '',
    categoryId: undefined,
    pageNumber: 1,
    pageSize: 10,
  });
  const totalCount = ref(0);

  const refreshPosts = async () => {
    const res = await postApi.list(query);
    posts.value = res.items;
    totalCount.value = res.totalCount;
  };

  const toSearch = () => {
    query.pageNumber = 1;
    refreshPosts();
  };

  const toReset = () => {
    query.titleContains = '';
    query.categoryId = undefined;
    query.pageNumber = 1;
    refreshPosts();
  };

  const submitForm = async () => {
    try {
      if (!(await formRef.value?.validate({}))) {
        return;
      }
      if (form.id) {
        posts.value = await postApi.update(form as Post);
      } else {
        posts.value = await postApi.add(form);
      }
      toast.add({
        title: 'Success',
        color: 'success',
        duration: 1500,
      });
      openForm.value = false;
    } catch (error) {
      console.error(error);
    }
  };

  const addPost = () => {
    formTitle.value = 'Add Post';
    Object.assign(form, defaultForm);
    openForm.value = true;
  };

  const editPost = (Post: Post) => {
    formTitle.value = 'Update Post';
    Object.assign(form, defaultForm, Post);
    openForm.value = true;
  };
  const deletePost = async (id: number) => {
    try {
      posts.value = await postApi.del(id);
      toast.add({
        title: 'Success',
        color: 'success',
        duration: 1500,
      });
    } catch (error) {}
  };

  const columns: TableColumn<Post>[] = [
    {
      accessorKey: 'id',
      header: 'ID',
      meta: {
        style: {
          th: { width: '90px', textAlign: 'center' },
          td: { width: '90px', textAlign: 'center' },
        },
      },
    },
    {
      accessorKey: 'title',
      header: 'Title',
      meta: {
        style: {
          th: { textAlign: 'center' },
          td: { textAlign: 'center' },
        },
      },
    },
    {
      accessorKey: 'categoryName',
      header: 'Category',
      meta: {
        style: {
          th: { width: '110px', textAlign: 'center' },
          td: { width: '110px', textAlign: 'center' },
        },
      },
    },
    {
      accessorKey: 'status',
      header: 'Status',
      meta: {
        style: {
          th: { width: '90px', textAlign: 'center' },
          td: { width: '90px', textAlign: 'center' },
        },
      },
      cell: ({ row }) => {
        return h('div', { class: 'flex gap-1 justify-center items-center' }, [
          row.original.status === Status.Draft
            ? h('span', { class: 'text-yellow-500 font-semibold' }, 'Draft')
            : row.original.status === Status.Published
            ? h('span', { class: 'text-green-500 font-semibold' }, 'Published')
            : h('span', { class: 'text-gray-500 font-semibold' }, 'Archived'),
        ]);
      },
    },
    {
      accessorKey: 'authorName',
      header: 'Author',
      meta: {
        style: {
          th: { textAlign: 'center' },
          td: { textAlign: 'center' },
        },
      },
    },
    {
      accessorKey: 'createdAt',
      header: 'Created At',
      meta: {
        style: {
          th: { textAlign: 'center' },
          td: { textAlign: 'center' },
        },
      },
    },
    {
      accessorKey: 'updatedAt',
      header: 'Updated At',
      meta: {
        style: {
          th: { textAlign: 'center' },
          td: { textAlign: 'center' },
        },
      },
    },
    {
      accessorKey: '',
      header: 'Action',
      meta: {
        style: {
          th: { width: '160px', textAlign: 'center' },
          td: { width: '160px', textAlign: 'center' },
        },
      },
      cell: ({ row }) => {
        return h('div', { class: 'flex gap-1 justify-center items-center' }, [
          h(
            UButton,
            {
              color: 'warning',
              onClick: () => editPost(row.original),
            },
            'Edit'
          ),
          h(UPopover, null, {
            default: () =>
              h(UButton, {
                label: 'Delete',
                color: 'error',
              }),
            content: ({ close }: { close: () => void }) =>
              h('div', { class: 'p-3 space-y-2 w-48' }, [
                h('p', 'Are you sure?'),
                h('div', { class: 'flex justify-end gap-2' }, [
                  h(UButton, {
                    size: 'xs',
                    variant: 'ghost',
                    label: 'Cancel',
                    onClick: close,
                  }),

                  h(UButton, {
                    size: 'xs',
                    variant: 'ghost',
                    label: 'OK',
                    onClick: () => {
                      deletePost(row.original.id);
                      close();
                    },
                  }),
                ]),
              ]),
          }),
        ]);
      },
    },
  ];
</script>
