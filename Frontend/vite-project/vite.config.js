import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

// https://vitejs.dev/config/
export default defineConfig({
  server: {
    host: "0.0.0.0",
    proxy: {
      '/api': {
        target: 'http://localhost:5257',
        changeOrigin: true,
        secure: true,
        ws: true,
      }
    },
  },
  plugins: [react()],

});
