import { defineConfig } from '@hey-api/openapi-ts';

export default defineConfig({
  input: '../assets/Dashboard.json',
  output: {
    format: 'prettier',
    lint: 'eslint',
    path: './src/shared/generated',
  }, plugins: [
    {
      enums: 'javascript',
      name: '@hey-api/typescript',
    }
  ]
});
