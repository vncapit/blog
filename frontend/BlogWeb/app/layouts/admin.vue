<template>
  <div class="min-h-screen min-w-full bg-neutral-100 dark:bg-neutral-900">
    <UHeader to="/admin/" title="Blog Admin">
      <UNavigationMenu :items="items" />
      <template #right>
        <UColorModeButton />
      </template>
    </UHeader>
    <UMain class="p-4">
      <slot />
    </UMain>
  </div>
</template>

<script setup lang="ts">
  const items = [
    { label: 'Dashboard', to: '/admin' },
    { label: 'Categories', to: '/admin/category' },
    { label: 'Posts', to: '/admin/post' },
  ];
</script>
