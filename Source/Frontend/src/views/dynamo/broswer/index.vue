<template>
  <div class="w-full box-border h-full flex flex-col flex-nowrap gap-5 justify-between flex-1">
    <n-radio-group v-model:value="orderValue" :on-update:value="handleUpdateValue" :loading="loading">
      <n-radio-button v-for="order in orders" :key="order.value" :value="order.value" :label="order.label" />
    </n-radio-group>
    <div class="flex-1">
      <n-scrollbar style="max-height: 1000px;" trigger="none">
        <n-list hoverable clickable>
          <n-list-item v-for="packageObj in packages" :key="packageObj.id">
            <n-thing :title="packageObj.name" content-style="margin-top: 10px;">
              <template #description>
                <n-flex>
                  <n-tag :bordered="false" type="info" size="small">
                    {{ packageObj.createdDate }}
                  </n-tag>
                  <n-tag :bordered="false" type="info" size="small">
                    {{ packageObj.updatedDate }}
                  </n-tag>
                  <n-tag :bordered="false" type="info" size="small">
                    {{ packageObj.downloads }}
                  </n-tag>
                  <n-tag :bordered="false" type="info" size="small">
                    {{ packageObj.votes }}
                  </n-tag>
                </n-flex>
              </template>
              <n-ellipsis style="max-width: 800px">
                {{ packageObj.description }}
              </n-ellipsis>
            </n-thing>
          </n-list-item>
        </n-list>
      </n-scrollbar>
    </div>

    <n-pagination :item-count="packageCount" v-model:value="pageRef" :on-update:page="handlePageChange">

    </n-pagination>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, watch, watchEffect } from 'vue'
import { useRoute, useRouter } from 'vue-router';
import { useMessage, useLoadingBar } from 'naive-ui'


import { getPackagePagesAsync, type PackageDTO } from '@/api/packages';

const message = useMessage()
const currentRoute = useRoute()
const router = useRouter()
const loadingBar = useLoadingBar()
const { orderBy, keyword, page, pageSize } = currentRoute.query;

const keywordRef = ref<string>(keyword as any as string)
const pageRef = ref<number>(page as any as number);
const pageSizeRef = ref<number>(pageSize as any as number)
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

const orderValue = ref<string>(orderBy as any as string || 'default')

async function getPacakgePages(keyword?: string, pageIndex: number = 1, size: number = 30) {
  try {
    loadingBar.start()
    var httpResponse = await getPackagePagesAsync(keyword, pageIndex, size)
    console.log(httpResponse)
    if (httpResponse.succeed) {
      packages.value = httpResponse.response.data
      packageCount.value = httpResponse.response.dataCount
    }
    else {
      throw new Error(httpResponse.message)
    }
  } catch (error: any) {
    message.error(error.message);
  }
  finally {
    loadingBar.finish()
  }
}
function handleUpdateValue(value: string) {
  orderValue.value = value
}
function handlePageChange(newPage: number) {
  router.push({
    name: 'package-broswer',
    query: {
      keyword: currentRoute.query.keyword,
      page: newPage,
      pageSize: currentRoute.query.pageSize,
      orderBy: currentRoute.query.orderBy
    }
  })
}


onMounted(async () => {
  await getPacakgePages(keywordRef.value, pageRef.value, pageSizeRef.value)
})

// watch(() => currentRoute.fullPath, async () => {
//   await getPacakgePages(keywordRef.value, pageRef.value, pageSizeRef.value)
// })

// watchEffect(async () => {
//   await getPacakgePages(keywordRef.value, pageRef.value, pageSizeRef.value)
// })
</script>
