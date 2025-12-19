<template>
  <UCard class="w-[400px] p-6">
    <form @submit.prevent="">
      <div class="mb-4">
        <UInput
          v-model="username"
          label="Username"
          type="text"
          required
          placeholder="Enter your username"
          class="w-full"
        />
      </div>
      <div class="mb-6">
        <UInput
          v-model="password"
          label="Password"
          type="password"
          required
          placeholder="Enter your password"
          class="w-full"
        />
      </div>
      <UButton
        type="button"
        label="Login"
        color="primary"
        class="w-full flex flex-col"
        @click="submit"
      />
    </form>
  </UCard>
</template>

<script setup lang="ts">
  import { ref } from 'vue';
  import { useAuthStore } from '@/stores/authStore';
  definePageMeta({
    layout: 'auth',
  });
  const username = ref('');
  const password = ref('');
  const submit = () => {
    console.log('Username:', username.value);
    console.log('Password:', password.value);
    const authStore = useAuthStore();
    if (process.client) {
      console.log('sdfsdf');
      authStore.login(username.value, password.value);
    }
  };
</script>
