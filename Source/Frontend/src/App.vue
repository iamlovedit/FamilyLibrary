<template>
  <n-config-provider :theme="theme" :locale="zhCN" :date-locale="dateZhCN">
    <n-global-style />
    <app-provider>
      <router-view />
    </app-provider>
  </n-config-provider>
</template>

<script setup lang="ts">

import { RouterView } from 'vue-router';
import { darkTheme, lightTheme, zhCN, dateZhCN } from 'naive-ui';
import { computed } from 'vue';

import { useAppStore } from './stores/modules/app';
import { AppProvider } from '@/components/Application';

const appStore = useAppStore();

const theme = computed(() => {
  return appStore.theme === 'dark' ? darkTheme : lightTheme
});
</script>

<style scoped>
.n-config-provider {
  height: 100%;
}
</style>