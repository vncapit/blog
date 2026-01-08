<script setup lang="ts">
  import type { EditorCustomHandlers, EditorToolbarItem } from '@nuxt/ui';
  import type { Editor } from '@tiptap/core';
  import { ImageUpload } from './EditorImageUpload';

  const customHandlers = {
    imageUpload: {
      canExecute: (editor: Editor, cmd?: any) =>
        editor.can().insertContent({ type: 'imageUpload' }),
      execute: (editor: Editor) => editor.chain().focus().insertContent({ type: 'imageUpload' }),
      isActive: (editor: Editor) => editor.isActive('imageUpload'),
      isDisabled: undefined,
    },
  } satisfies EditorCustomHandlers;

  const items = [
    [
      {
        kind: 'imageUpload',
        icon: 'i-lucide-image',
        label: 'Add image',
        variant: 'soft',
      },
    ],
    [
      { kind: 'heading', level: 1, icon: 'i-lucide-heading-1' },
      { kind: 'heading', level: 2, icon: 'i-lucide-heading-2' },
      { kind: 'heading', level: 3, icon: 'i-lucide-heading-3' },
      { kind: 'mark', mark: 'bold', icon: 'i-lucide-bold' },
      { kind: 'mark', mark: 'italic', icon: 'i-lucide-italic' },
      { kind: 'bulletList', icon: 'i-lucide-list' },
      { kind: 'orderedList', icon: 'i-lucide-list-ordered' },
      { kind: 'blockquote', icon: 'i-lucide-quote' },
      { kind: 'link', icon: 'i-lucide-link' },
    ],
  ] satisfies EditorToolbarItem<typeof customHandlers>[][];
</script>

<template>
  <UEditor
    v-slot="{ editor }"
    :extensions="[ImageUpload]"
    :handlers="customHandlers"
    content-type="markdown"
    :ui="{ base: 'p-8 sm:px-16' }"
    class="w-full min-h-74"
  >
    <UEditorToolbar
      :editor="editor"
      :items="items"
      class="border-b border-muted py-2 px-8 sm:px-16 overflow-x-auto"
    />
  </UEditor>
</template>
