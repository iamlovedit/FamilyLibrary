<template>
  <n-layout :has-sider="false" class="h-full w-full flex-col flex">
    <page-header> </page-header>
    <div class="flex-1 flex flex-nowrap flex-col h-full">
      <router-view />
    </div>
    <n-layout-footer borderd> 蜀 </n-layout-footer>
  </n-layout>
</template>

<script setup lang="ts">
import PageHeader from './Header'
</script>

<style scoped>
.n-layout-footer {
  height: var(--footer-height);
  text-align: center;
  line-height: var(--footer-height);
}
</style>
