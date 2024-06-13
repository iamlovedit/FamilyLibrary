<template>
  <n-layout-header bordered class="px-3 flex flex-col justify-center">
    <n-menu
      mode="horizontal"
      responsive
      :options="menuOptions"
      v-model:value="activeKey"
      :on-update:value="handleClickItem"
    />
  </n-layout-header>
</template>

<script setup lang="ts">
import { computed } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import type { MenuOption } from 'naive-ui'

const menuOptions: MenuOption[] = [
  {
    label: '首页',
    key: 'home'
  },
  {
    label: '节点包',
    key: 'package-home'
  },
  {
    label: '族库',
    key: 'family-home'
  }
]
const currentRoute = useRoute()
const router = useRouter()
const activeKey = computed(() => currentRoute.name)

function handleClickItem(key: string) {
  router.push({
    name: key
  })
}
</script>

<style scoped>
.n-layout-header {
  height: var(--header-height);
}
</style>
