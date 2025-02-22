<template>
  <n-flex vertical>
    <n-card>
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
    </n-card>

    <n-card>
      <n-h1>{{ packageRef?.name }}</n-h1>
      <n-p>
        {{ packageRef?.description }}
      </n-p>
    </n-card>
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
    <n-pagination
      v-model:page="pageRef"
      :item-count="versionCountRef"
      v-model:page-size="pageSizeRef"
      :page-slot="8"
      :page-sizes="[10, 20, 30, 40]"
      @update:page="handlePageChange"
      @update:page-size="handleSizeChange"
      show-quick-jumper
      show-size-picker
    />
  </n-flex>
</template>

<script setup lang="ts">
import { ref, onMounted, watch } from 'vue'
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
const { page, pageSize } = currentRoute.query
const router = useRouter()
const message = useMessage()
const loadingBar = useLoadingBar()
const versionsRef = ref<PackageVersionDTO[]>([])
const packageRef = ref<PackageDTO>()
const versionCountRef = ref<number>()
const pageRef = ref<number>(Number(page) || 1)
const pageSizeRef = ref<number>(Number(pageSize) || 10)
function handleBack() {
  router.back()
}

async function handlePageChange(newPage: number) {
  await router.push({
    name: 'package-detail',
    params: {
      id: currentRoute.params.id
    },
    query: {
      page: newPage,
      pageSize: currentRoute.query.pageSize
    }
  })
}

async function handleSizeChange(pageSize: number) {
  await router.push({
    name: 'package-detail',
    params: {
      id: currentRoute.params.id
    },
    query: {
      page: currentRoute.query.page,
      pageSize: pageSize
    }
  })
}

onMounted(async () => {
  try {
    loadingBar.start()
    const id = currentRoute.params.id as string
    const [versionResponse, detailResponse] = await Promise.all([
      getPackageVersionPageAsync(id, pageRef.value, pageSizeRef.value),
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

watch(
  () => currentRoute.fullPath,
  async () => {
    try {
      loadingBar.start()
      const versionResponse = await getPackageVersionPageAsync(
        packageRef.value!.id,
        pageRef.value,
        pageSizeRef.value
      )
      if (versionResponse.succeed) {
        versionsRef.value = versionResponse.response.data
      } else {
        throw new Error(versionResponse.message)
      }
    } catch (error: any) {
      message.error(error.message)
    } finally {
      loadingBar.finish()
    }
  }
)
</script>
