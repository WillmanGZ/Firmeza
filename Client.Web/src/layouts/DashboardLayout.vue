<script setup lang="ts">
import AsideBar from '@/components/AsideBar.vue';
</script>

<template>
  <main class="flex flex-row">
    <AsideBar />
    <RouterView class="flex-1 max-h-svh overflow-y-scroll" />
  </main>
</template>
