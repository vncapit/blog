<template>
  <div class="flex flex-col flex-1">
    <h1 class="text-2xl font-bold mb-4">Category Management</h1>
    <div class="mt-2 mb-2">
      <UButton @click="addCategory" color="primary"> Add Category </UButton>
    </div>
    <UTable :data="categories || []" :columns="columns" class="flex-1" />

    <UModal v-model:open="openForm" :dismissible="false" :title="formTitle">
      <template #body>
        <UForm ref="formRef" :schema="schema" :state="form" class="space-y-4">
          <UFormField label="Category" name="name" required>
            <UInput v-model="form.name" class="w-full" />
          </UFormField>
        </UForm>
      </template>
      <template #footer>
        <div class="w-full flex justify-end gap-2">
          <UButton variant="text" @click="openForm = false">Cancel</UButton>
          <UButton @click="submitForm" form="category-form">Save</UButton>
        </div>
      </template>
    </UModal>
  </div>
</template>

<script setup lang="ts">
  import { h, resolveComponent } from 'vue';
  import type { TableColumn } from '@nuxt/ui';
  import { useCategoryApi } from '~/composables/api/admin/useCategoryApi';
  import { ref, reactive, onMounted } from 'vue';
  import * as z from 'zod';
  import type { Category } from '~/types';

  definePageMeta({
    layout: 'admin',
  });

  const toast = useToast();
  const categoryApi = useCategoryApi();

  const UButton = resolveComponent('UButton');
  const UPopover = resolveComponent('UPopover');

  const categories = ref<Category[]>([]);

  const formRef = useTemplateRef('formRef');
  const schema = z.object({
    name: z.string().min(1, 'Category name is required'),
  });

  const defaultForm = { id: undefined as number | undefined, name: '' };
  const openForm = ref(false);
  const formTitle = ref('Add Category');

  const form = reactive({ ...defaultForm });

  onMounted(async () => {
    categories.value = await categoryApi.list();
  });

  const submitForm = async () => {
    try {
      if (!(await formRef.value?.validate({}))) {
        return;
      }
      if (form.id) {
        categories.value = await categoryApi.update(form as Category);
      } else {
        categories.value = await categoryApi.add(form);
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

  const addCategory = () => {
    formTitle.value = 'Add category';
    Object.assign(form, defaultForm);
    openForm.value = true;
  };

  const editCategory = (category: Category) => {
    formTitle.value = 'Update category';
    Object.assign(form, defaultForm, category);
    openForm.value = true;
  };
  const deleteCategory = async (id: number) => {
    try {
      categories.value = await categoryApi.del(id);
      toast.add({
        title: 'Success',
        color: 'success',
        duration: 1500,
      });
    } catch (error) {}
  };

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
              color: 'warning',
              onClick: () => editCategory(row.original),
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
                      deleteCategory(row.original.id);
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
