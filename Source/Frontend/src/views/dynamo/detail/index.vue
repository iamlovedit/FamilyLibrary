<template>
  <n-flex vertical>
    <n-page-header @back="handleBack" class="w-full">
      <template #title>
        {{ packageRef?.name }}
      </template>
      <n-flex justify="space-around" size="large">
        <n-statistic label="发布版本" :value="`${versionCountRef} 个`" />
        <n-statistic label="下载" :value="`${packageRef?.downloads} 次`" />
        <n-statistic label="点赞" :value="`${packageRef?.votes} 次`" />
        <n-statistic label="发布时间" :value="`${packageRef?.createdDate}`" />
        <n-statistic label="更新时间" :value="`${packageRef?.updatedDate}`" />
      </n-flex>
    </n-page-header>
    <n-h1>{{ packageRef?.name }}</n-h1>
    <n-p>
      {{ packageRef?.description }}
    </n-p>
    <div class="flex-1">
      <n-list hoverable>
        <n-list-item v-for="version in versionsRef" :key="version.version">
          <template #prefix>
            <n-button
              text
              tag="a"
              :href="`${url}package/v1/${currentRoute.params.id}/${version.version}`"
              target="_blank"
              type="primary"
            >
              {{ version.version }}
            </n-button>
          </template>
          <template #Suffix> {{ version.createdDate }} </template>
        </n-list-item>
      </n-list>
    </div>
    <n-pagination />
  </n-flex>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { useMessage, useLoadingBar } from 'naive-ui'
import {
  getPackageVersionPageAsync,
  getPackageDetailAsync,
  type PackageVersionDTO,
  type PackageDTO
} from '@/api/dynamo'
const url = import.meta.env.VITE_APP_API_BASE_URL

const currentRoute = useRoute()
const router = useRouter()
const message = useMessage()
const loadingBar = useLoadingBar()
const versionsRef = ref<PackageVersionDTO[]>([])
const packageRef = ref<PackageDTO>()
const versionCountRef = ref<number>()
function handleBack() {
  router.back()
}

onMounted(async () => {
  try {
    loadingBar.start()
    const id = currentRoute.params.id as string
    const [versionResponse, detailResponse] = await Promise.all([
      getPackageVersionPageAsync(id),
      getPackageDetailAsync(id)
    ])
    if (versionResponse.succeed && detailResponse.succeed) {
      versionsRef.value = versionResponse.response.data
      versionCountRef.value = versionResponse.response.dataCount
      packageRef.value = detailResponse.response
    } else {
      const errorMessage = versionResponse.message || detailResponse.message || '未知错误'
      throw new Error(errorMessage)
    }
  } catch (error: any) {
    message.error(error.message)
  } finally {
    loadingBar.finish()
  }
})
</script>

<style scoped></style>
