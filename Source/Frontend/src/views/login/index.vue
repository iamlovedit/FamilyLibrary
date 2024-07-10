<template>
  <div
    class="flex flex-col h-full overflow-auto flex-nowrap justify-center mx-auto gap-4 min-w-320px max-w-384px"
  >
    <n-form ref="formRef" label-placement="left" size="large" :model="formInline" :rules="rules">
      <n-form-item path="username">
        <n-input v-model:value="formInline.username" placeholder="请输入用户名">
          <template #prefix>
            <n-icon :size="iconSize" :color="iconColor">
              <ChartPerson24Regular />
            </n-icon>
          </template>
        </n-input>
      </n-form-item>
      <n-form-item path="password">
        <n-input
          v-model:value="formInline.password"
          type="password"
          showPasswordOn="click"
          placeholder="请输入密码"
        >
          <template #prefix>
            <n-icon :size="iconSize" :color="iconColor">
              <Password />
            </n-icon>
          </template>
        </n-input>
      </n-form-item>
      <n-form-item class="default-color">
        <div class="flex justify-between">
          <div class="flex-initial">
            <n-checkbox v-model:checked="remember">记住账号</n-checkbox>
          </div>
        </div>
      </n-form-item>
      <n-form-item>
        <n-button type="primary" @click="handleLogin" size="large" :loading="loading" block>
          登录
        </n-button>
      </n-form-item>
    </n-form>
  </div>
</template>

<script setup lang="ts">
import { useRoute, useRouter } from 'vue-router'
import { ref } from 'vue'
import { useMessage, type FormValidationError, useLoadingBar } from 'naive-ui'
import { ChartPerson24Regular } from '@vicons/fluent'
import { Password } from '@vicons/carbon'

import { useUserStore } from '@/stores/modules/user'
import { useAuthStore } from '@/stores/modules/auth'
import type { LoginDTO } from '@/stores/modules/auth/helper'

const iconSize: number = 20
const iconColor: string = '#808695'

const userStore = useUserStore()
const authStore = useAuthStore()
const router = useRouter()
const currentRoute = useRoute()
const formRef = ref()
const loading = ref(false)
const remember = ref<boolean>(authStore.remember)
const message = useMessage()
const loadingBar = useLoadingBar()
const formInline = ref<LoginDTO>(authStore.loginDTO)

const rules = {
  username: { required: true, message: '请输入用户名', trigger: 'blur' },
  password: { required: true, message: '请输入密码', trigger: 'blur' }
}

function handleLogin(e: Event): void {
  e.preventDefault()
  formRef.value.validate(async (errors: Array<FormValidationError>) => {
    if (!errors) {
      loadingBar.start()
      message.loading('登录中...')
      loading.value = true
      try {
        const httpResponse = await userStore.loginAsync(formInline.value)
        if (httpResponse?.succeed) {
          authStore.setRemember(remember.value)
          if (remember.value) {
            authStore.setLoginDTO(formInline.value)
          } else {
            authStore.clearLoginDTO()
          }
          await userStore.getCurrentUserAsync()
          message.destroyAll()
          message.success('登录成功，即将进入系统')
          await router.push({
            name: 'home'
          })
        } else {
          loadingBar.error()
          throw new Error(httpResponse?.message || '登录失败')
        }
      } catch (error: any) {
        loadingBar.error()
        message.info(error.message)
      } finally {
        loading.value = false
        loadingBar.finish()
        message.destroyAll()
      }
    } else {
      message.error('请填写完整登录信息')
    }
  })
}
</script>
