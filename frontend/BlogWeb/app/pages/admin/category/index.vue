<template>
  <div class="min-w-full min-h-full">
    <h1 class="text-2xl font-bold mb-4">Category Management</h1>
    <UTable :data="data" :columns="columns" class="flex-1" />
  </div>
</template>

<script setup lang="ts">
  import { h, resolveComponent } from 'vue';
  import type { TableColumn } from '@nuxt/ui';
  import { ref } from 'vue';
  definePageMeta({
    layout: 'admin',
  });

  type Category = {
    id: number;
    name: string;
  };

  const UButton = resolveComponent('UButton');

  const data = ref([
    { id: 1, name: 'Category 1' },
    { id: 2, name: 'Category 2' },
  ]);

  const columns: TableColumn<Category>[] = [
    { accessorKey: 'id', header: 'ID' },
    { accessorKey: 'name', header: 'Name' },
    {
      accessorKey: '',
      header: 'Action',
      cell: ({ row }) => {
        return h('div', { class: 'flex gap-1' }, [
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
              onClick: () => editCategory(row.original.id),
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
