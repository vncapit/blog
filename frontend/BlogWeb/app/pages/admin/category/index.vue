<template>
  <div class="flex flex-col flex-1">
    <h1 class="text-2xl font-bold mb-4">Category Management</h1>
    <UTable :data="categories || []" :columns="columns" class="flex-1" />
  </div>
</template>

<script setup lang="ts">
  import { h, resolveComponent } from 'vue';
  import type { TableColumn } from '@nuxt/ui';
  import { useCategoryApi } from '~/composables/api/admin/useCategoryApi';
  import { ref, onMounted } from 'vue';

  definePageMeta({
    layout: 'admin',
  });

  type Category = {
    id: number;
    name: string;
  };

  const UButton = resolveComponent('UButton');

  const categories = ref<Category[]>([]);

  onMounted(async () => {
    categories.value = await useCategoryApi();
  });

  const columns: TableColumn<Category>[] = [
    {
      accessorKey: 'id',
      header: 'ID',
      meta: {
        style: {
          th: { width: '110px', textAlign: 'center' },
          td: { width: '110px', textAlign: 'center' },
        },
      },
    },
    {
      accessorKey: 'name',
      header: 'Name',
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
          th: { width: '200px', textAlign: 'center' },
          td: { width: '200px', textAlign: 'center' },
        },
      },
      cell: ({ row }) => {
        return h('div', { class: 'flex gap-1 justify-center items-center' }, [
          h(
            UButton,
            {
              color: 'secondary',
              onClick: () => editCategory(row.original.id),
            },
            'Edit'
          ),
          h(
            UButton,
            {
              color: 'error',
              onClick: () => deleteCategory(row.original.id),
            },
            'Delete'
          ),
        ]);
      },
    },
  ];

  const editCategory = (id: number) => {
    console.log('Edit category with ID:', id);
  };
  const deleteCategory = (id: number) => {
    console.log('Delete category with ID:', id);
  };
</script>
