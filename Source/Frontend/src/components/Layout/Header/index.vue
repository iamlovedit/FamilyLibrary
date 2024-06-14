<template>
  <n-layout-header bordered class="px-3 flex flex-col justify-center">
    <n-flex justify="space-between" class="w-full">
      <div>
        <n-menu mode="horizontal" responsive :options="menuOptions" v-model:value="activeKey"
          :on-update:value="handleClickItem" />
      </div>
      <div>
        <n-dropdown v-if="userStore.user?.name">
          <n-button>
            个人资料
          </n-button>
        </n-dropdown>
        <n-button v-else>
          登录
        </n-button>
      </div>
    </n-flex>
  </n-layout-header>
</template>

<script setup lang="ts">
import { computed } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import type { MenuOption } from 'naive-ui'
import { useUserStore } from '@/stores/modules/user';

const userStore = useUserStore()

const menuOptions: MenuOption[] = [
  {
    label: '首页',
    key: 'home'
  },
  {
    label: '节点包',
    key: 'package'
  },
  {
    label: '族库',
    key: 'family'
  }
]
const currentRoute = useRoute()
const router = useRouter()
const activeKey = computed(() => currentRoute.matched[0]?.name)
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
