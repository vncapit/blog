// @ts-check
import withNuxt from './.nuxt/eslint.config.mjs';
import tseslint from 'typescript-eslint';

export default withNuxt(
  // Your custom configs here
  {
    files: ['**/*.ts', '**/*.tsx', '**/*.vue'],
    languageOptions: {
      parser: tseslint.parser,
    },
  }
);
