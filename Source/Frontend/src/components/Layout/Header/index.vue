<template>
  <n-layout-header bordered class="px-3 flex flex-col justify-center">
    <n-flex justify="space-between" class="w-full">
      <div>
        <n-menu
          mode="horizontal"
          responsive
          :options="menuOptions"
          v-model:value="activeKey"
          :on-update:value="handleClickItem"
        />
      </div>
      <n-flex>
        <n-dropdown v-if="userStore.user?.username" :options="dropdownOptions">
          <n-button> 个人资料 </n-button>
        </n-dropdown>
        <n-button-group v-else>
          <n-button :bordered="false" @click="handleLoginClick"> 登录 </n-button>
          <n-button :bordered="false" @click="handleRegisterClick"> 注册</n-button>
        </n-button-group>
        <n-button secondary circle @click="appStore.switchTheme">
          <n-icon :component="themeIcon" />
        </n-button>
      </n-flex>
    </n-flex>
  </n-layout-header>
</template>

<script setup lang="ts">
import { type Component, computed, h } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import type { MenuOption, DropdownOption } from 'naive-ui'
import { NIcon } from 'naive-ui'
import { DarkModeRound, LightModeRound } from '@vicons/material'
import {
  PersonCircleOutline as UserIcon,
  Pencil as EditIcon,
  LogOutOutline as LogoutIcon
} from '@vicons/ionicons5'

import { useUserStore } from '@/stores/modules/user'
import { useAppStore } from '@/stores/modules/app'

const appStore = useAppStore()

const themeIcon = computed(() => {
  if (appStore.theme == 'dark') {
    return DarkModeRound
  } else {
    return LightModeRound
  }
})

const renderIcon = (icon: Component) => {
  return () => {
    return h(NIcon, null, {
      default: () => h(icon)
    })
  }
}

const dropdownOptions: DropdownOption[] = [
  {
    label: '用户资料',
    key: 'profile',
    icon: renderIcon(UserIcon)
  },
  {
    label: '编辑用户资料',
    key: 'editProfile',
    icon: renderIcon(EditIcon)
  },
  {
    label: '退出登录',
    key: 'logout',
    icon: renderIcon(LogoutIcon)
  }
]

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

function handleRegisterClick() {
  router.push({
    name: 'register'
  })
}

function handleLoginClick() {
  router.push({
    name: 'login'
  })
}
</script>

<style scoped>
.n-layout-header {
  height: var(--header-height);
}
</style>
