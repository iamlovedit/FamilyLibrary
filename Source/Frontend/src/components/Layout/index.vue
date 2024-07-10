<template>
  <div class="h-full w-full flex-col flex flex-nowrap justify-between">
    <page-header />
    <router-view class="flex-1 flex flex-nowrap flex-col" />
    <n-layout-footer borderd> 蜀ICP备2022029729号-1 </n-layout-footer>
  </div>
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
