<template>
  <n-flex>
    <n-scrollbar class="w-300px min-w-200px max-h-1200px">
      <n-tree expand-on-click :data="categoriesRef" key-field="id" label-field="name"> </n-tree>
    </n-scrollbar>
    <n-flex vertical class="flex-1 gap-8">
      <n-dynamic-tags v-model:value="tagsRef" />
      <n-input-group>
        <n-input :style="{ width: '50%' }" />
        <n-button type="primary"> 搜索 </n-button>
      </n-input-group>
      <div class="flex-1 flex flex-wrap gap-4 w-full m-auto" hoverable>
        <family-card
          v-for="family in familiesRef"
          :key="family.id"
          :name="family.name"
          cover="https://gw.alipayobjects.com/zos/antfincdn/aPkFc8Sj7n/method-draw-image.svg"
          class="family-container"
        >
        </family-card>
      </div>
      <n-pagination v-model:page="pageRef" />
    </n-flex>
  </n-flex>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import {
  type FamilyCategory,
  type FamilyBasic,
  getFamilyCategories,
  getFamiliesPagePromise
} from '@/api/family'
import { useMessage, useLoadingBar } from 'naive-ui'
import FamilyCard from '@/components/FamilyCard/index.vue'

const currentRoute = useRoute()
const router = useRouter()
const message = useMessage()
const loadingBar = useLoadingBar()
const categoriesRef = ref<FamilyCategory[]>([])
const tagsRef = ref<string[]>(['教师', '程序员'])
const familiesRef = ref<FamilyBasic[]>([])
const { page, pageSize } = currentRoute.query
const pageRef = ref<number>(Number(page || 1))
const pageSizeRef = ref<number>(Number(pageSize) || 30)
async function getCategories() {
  try {
    loadingBar.start()
    const httpResponse = await getFamilyCategories()
    if (httpResponse.succeed) {
      categoriesRef.value = httpResponse.response
    } else {
      throw new Error(httpResponse.message)
    }
  } catch (error: any) {
    message.error(error.message)
  } finally {
    loadingBar.finish()
  }
}

async function getFamiliesPage(
  keyword?: string,
  categoryId?: string,
  pageIndex: number = 1,
  pageSize: number = 30
) {
  try {
    loadingBar.start()
    const httpResponse = await getFamiliesPagePromise(keyword, categoryId, pageIndex, pageSize)
    if (httpResponse.succeed) {
      familiesRef.value = httpResponse.response.data
    } else {
      throw new Error(httpResponse.message)
    }
  } catch (error: any) {
    message.error(error.message)
  } finally {
    loadingBar.finish()
  }
}

onMounted(async () => {
  await getCategories()
  await getFamiliesPage()
})
</script>
