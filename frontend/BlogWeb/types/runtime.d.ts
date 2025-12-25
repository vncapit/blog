declare module '#app' {
  interface NuxtApp {
    $api: <T = any>(url: string, options?: any) => Promise<T>;
  }
}

export {};
