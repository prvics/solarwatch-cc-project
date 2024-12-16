import { defineConfig } from "vite";
import react from "@vitejs/plugin-react";
const backendUrl = process.env.DOCKER_ENV
    ? "http://backend:5222" // Docker environment
    : "http://localhost:5222"; // Local development

// https://vite.dev/config/
export default defineConfig({
  plugins: [react()],
  build: {
    outDir: "dist",
    emptyOutDir: true,
  },
  server: {
    proxy: {
      "/api": {
        target: backendUrl,
        changeOrigin: true,
        secure: false,
      },
    },
  },
});
