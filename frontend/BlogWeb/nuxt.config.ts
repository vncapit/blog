// https://nuxt.com/docs/api/configuration/nuxt-config
export default defineNuxtConfig({
  modules: ['@nuxt/eslint', '@nuxt/ui', '@pinia/nuxt', '@nuxt/icon'],
  devtools: {
    enabled: true,
  },

  css: ['~/assets/css/main.css'],

  routeRules: {
    '/': { prerender: true },
  },

  compatibilityDate: '2025-01-15',

  eslint: {
    config: {
      stylistic: {
        commaDangle: 'never',
        braceStyle: '1tbs',
      },
    },
  },

  runtimeConfig: {
    public: {
      apiBase: '',
    },
  },

  icon: {
    provider: 'iconify',
    collections: ['lucide'],
  },

  nitro: {
    devProxy: {
      '/api': {
        target: 'http://localhost:5233/api',
        changeOrigin: true,
      },
    },
  },

  vite: {
    optimizeDeps: {
      include: ['prosemirror-state'],
    },
  },
});
