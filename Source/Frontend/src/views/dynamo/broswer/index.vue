<template>
  <div class="w-full box-border h-full flex flex-col flex-nowrap gap-5 justify-between flex-1">
    <n-radio-group v-model:value="orderRef" :on-update:value="handleUpdateValue" :loading="loading">
      <n-radio-button v-for="order in orders" :key="order.value" :value="order.value" :label="order.label" />
    </n-radio-group>
    <div class="flex-1">
      <n-scrollbar style="max-height: 800px" trigger="none">
        <n-list hoverable show-divider>
          <n-list-item v-for="packageObj in packages" :key="packageObj.id">
            <n-thing :title="packageObj.name" content-style="margin-top: 10px;">
              <template #description>
                <n-flex>
                  <n-tag :bordered="false" type="info">
                    {{ packageObj.createdDate }}
                    <template #icon>
                      <n-icon :component="NewReleasesOutlined" />
                    </template>
                  </n-tag>
                  <n-tag :bordered="false" type="info">
                    {{ packageObj.updatedDate }}
                    <template #icon>
                      <n-icon :component="UpdateRound" />
                    </template>
                  </n-tag>
                  <n-tag :bordered="false" type="info">
                    {{ packageObj.downloads }}
                    <template #icon>
                      <n-icon :component="FileDownloadDoneSharp" />
                    </template>
                  </n-tag>
                  <n-tag :bordered="false" type="info">
                    {{ packageObj.votes }}
                    <template #icon>
                      <n-icon :component="ThumbUpAltOutlined" />
                    </template>
                  </n-tag>
                </n-flex>
              </template>
              <n-ellipsis style="max-width: 800px">
                {{ packageObj.description }}
              </n-ellipsis>
            </n-thing>
            <template #suffix>
              <n-button @click="() => handleDetailClick(packageObj)">详情</n-button>
            </template>
          </n-list-item>
        </n-list>
      </n-scrollbar>
    </div>
    <n-pagination :item-count="packageCount" v-model:page="pageRef" :on-update:page="handlePageChange" :page-slot="8"
      v-model:page-size="pageSizeRef" show-quick-jumper />
  </div>
</template>

<script setup lang="ts">
import { ref, watchEffect, watch } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { useMessage, useLoadingBar } from 'naive-ui'
import {
  NewReleasesOutlined,
  UpdateRound,
  FileDownloadDoneSharp,
  ThumbUpAltOutlined
} from '@vicons/material'

import { getPackagePagesAsync, type PackageDTO } from '@/api/dynamo'

const message = useMessage()
const currentRoute = useRoute()
const router = useRouter()
const loadingBar = useLoadingBar()
const { orderBy, keyword, page, pageSize } = currentRoute.query
const keywordRef = ref<string>(keyword as string)
const pageRef = ref<number>(Number(page) || 1)
const pageSizeRef = ref<number>(Number(pageSize) || 30)
const packageCount = ref<number>()
const packages = ref<PackageDTO[]>([])
const loading = ref<boolean>()

const orders = [
  {
    value: 'default',
    label: '默认'
  },
  {
    value: 'name',
    label: '名称'
  },
  {
    value: 'downloads',
    label: '下载'
  },
  {
    value: 'votes',
    label: '投票'
  },
  {
    value: 'created',
    label: '发布时间'
  }
]

const orderRef = ref<string>((orderBy as string) || 'default')

async function getPacakgePages(
  keyword?: string,
  pageIndex: number = 1,
  size: number = 30,
  orderBy?: string
) {
  try {
    loadingBar.start()
    var httpResponse = await getPackagePagesAsync(keyword, pageIndex, size)
    if (httpResponse.succeed) {
      packages.value = httpResponse.response.data
      packageCount.value = httpResponse.response.dataCount
    } else {
      throw new Error(httpResponse.message)
    }
  } catch (error: any) {
    message.error(error.message)
  } finally {
    loadingBar.finish()
  }
}
async function handleUpdateValue(value: string) {
  await router.push({
    name: 'package-broswer',
    query: {
      keyword: currentRoute.query.keyword,
      page: currentRoute.query.page,
      pageSize: currentRoute.query.pageSize,
      orderBy: value
    }
  })
}
async function handlePageChange(newPage: number) {
  await router.push({
    name: 'package-broswer',
    query: {
      keyword: currentRoute.query.keyword,
      page: newPage,
      pageSize: currentRoute.query.pageSize,
      orderBy: currentRoute.query.orderBy
    }
  })
}

async function handleDetailClick(packageObj: PackageDTO) {
  await router.push({
    name: 'package-detail',
    params: {
      id: packageObj.id
    },
    query: {
      page: 1
    }
  })
}

watch(
  () => currentRoute.fullPath,
  () => {
    keywordRef.value = currentRoute.query.keyword as string
    pageRef.value = Number(currentRoute.query.page)
    pageSizeRef.value = Number(currentRoute.query.pageSize)
    orderRef.value = currentRoute.query.orderBy as string
  }
)

watchEffect(async () => {
  await getPacakgePages(keywordRef.value, pageRef.value, pageSizeRef.value, orderRef.value)
})
</script>
