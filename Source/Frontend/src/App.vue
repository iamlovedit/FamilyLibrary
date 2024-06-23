<template>
  <n-config-provider :theme="theme" :locale="zhCN" :date-locale="dateZhCN">
    <n-global-style />
    <app-provider>
      <layout />
    </app-provider>
  </n-config-provider>
</template>

<script setup lang="ts">
import { darkTheme, lightTheme, zhCN, dateZhCN } from 'naive-ui'
import { computed } from 'vue'
import { useEventListener } from '@vueuse/core'

import { useAppStore } from './stores/modules/app'
import { AppProvider } from '@/components/Application'
import Layout from '@/components/Layout/index.vue'

const appStore = useAppStore()

const theme = computed(() => {
  return appStore.theme === 'dark' ? darkTheme : lightTheme
})
function setFullHeight(): void {
  const headerHeight = parseInt(
    getComputedStyle(document.documentElement).getPropertyValue('--header-height'),
    10
  )
  const windowHeight = window.innerHeight
  document.documentElement.style.setProperty('--full-height', `${windowHeight - headerHeight}px`)
}
useEventListener('resize', setFullHeight)
</script>

<style scoped>
.n-config-provider  {
  height: 100vh;
  width: 100%;
}
</style>
