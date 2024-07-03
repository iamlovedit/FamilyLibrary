<template>
  <n-flex class="flex-1">
    <n-scrollbar class="w-300px min-w-200px max-h-1200px">
      <n-tree
        expand-on-click
        :data="categoriesRef"
        key-field="id"
        label-field="name"
        selectable
        v-model:selected-keys="selectedKeysRef"
        @update:selected-keys="handleSelectedChanged"
      >
      </n-tree>
    </n-scrollbar>
    <n-flex vertical class="flex-1 gap-8">
      <n-flex>
        <n-tag
          v-for="tag in tagsRef"
          :key="tag.value"
          :type="tag.type"
          closable
          @close="handleTagClosed(tag)"
        >
          {{ tag.value }}
        </n-tag>
      </n-flex>
      <n-radio-group
        v-model:value="orderRef"
        @update:value="handleOrderByChanged"
        :loading="loading"
      >
        <n-radio-button
          v-for="order in orders"
          :key="order.value"
          :value="order.value"
          :label="order.label"
        />
      </n-radio-group>
      <n-input-group>
        <n-input :style="{ width: '50%' }" v-model:value="keywordRef" />
        <n-button type="primary" ghost @click="handleKeywordSearch"> 搜索 </n-button>
      </n-input-group>
      <div
        class="flex-1 flex flex-wrap gap-4 w-full m-auto"
        hoverable
        v-if="familiesRef.length !== 0"
      >
        <family-card
          v-for="family in familiesRef"
          :key="family.id"
          :name="family.name"
          cover="https://gw.alipayobjects.com/zos/antfincdn/aPkFc8Sj7n/method-draw-image.svg"
        >
        </family-card>
      </div>
      <n-empty v-else size="huge" description="暂无数据" />
      <n-pagination
        v-model:page="pageRef"
        show-quick-jumper
        show-size-picker
        :page-slot="8"
        @update:page="handlePageChange"
        v-model:page-size="pageSizeRef"
        :page-sizes="[20, 30, 40]"
        @update:page-size="handleSizeChange"
      />
    </n-flex>
  </n-flex>
</template>

<script setup lang="ts">
import { ref, onMounted, watch } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import {
  type FamilyCategory,
  type FamilyBasic,
  getFamilyCategories,
  getFamiliesPagePromise
} from '@/api/family'
import { useMessage, useLoadingBar, type TreeOption } from 'naive-ui'
import FamilyCard from '@/components/FamilyCard/index.vue'

interface TagInfo {
  value: string
  type: 'info' | 'success'
}

const currentRoute = useRoute()
const { categoryId, orderBy, keyword, page, pageSize } = currentRoute.query
const router = useRouter()
const message = useMessage()
const loadingBar = useLoadingBar()
const categoriesRef = ref<FamilyCategory[]>([])
const tagsRef = ref<TagInfo[]>([])
const familiesRef = ref<FamilyBasic[]>([])
const pageRef = ref<number>(Number(page || 1))
const pageSizeRef = ref<number>(Number(pageSize) || 30)
const loading = ref<boolean>()
const orderRef = ref<string>((orderBy as string) || 'default')
const keywordRef = ref<string>(keyword as string)
const selectedKeysRef = ref<string[]>([])
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
    label: '收藏'
  },
  {
    value: 'createdDate',
    label: '上传时间'
  }
]

async function handleTagClosed(tag: TagInfo) {
  tagsRef.value = tagsRef.value.filter((t) => t.value !== tag.value)
}

async function handleSelectedChanged(
  keys: Array<string | number>,
  option: Array<TreeOption | null>
) {
  handleTags({
    type: 'info',
    value: option[0]?.name as string
  })

  await router.push({
    name: 'family-browser',
    query: {
      keyword: currentRoute.query.keyword,
      categoryId: keys[0],
      page: currentRoute.query.page,
      pageSize: currentRoute.query.pageSize,
      order: currentRoute.query.order
    }
  })
}

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

function handleTags(tag: TagInfo) {
  if (tag.value) {
    if (tagsRef.value.some((t) => t.type === tag.type)) {
      tagsRef.value = tagsRef.value.map((t) => {
        if (t.type === tag.type) {
          return { ...t, value: tag.value }
        } else {
          return t
        }
      })
    } else {
      tagsRef.value.push(tag)
    }
  }
}

function findItemByCondition(
  items: FamilyCategory[],
  condition: (item: FamilyCategory) => boolean
): FamilyCategory | null {
  for (const item of items) {
    if (condition(item)) {
      return item
    }
    if (item.children && item.children.length > 0) {
      const foundItem: FamilyCategory | null = findItemByCondition(item.children, condition)
      if (foundItem) {
        return foundItem
      }
    }
  }
  return null
}

async function handleSizeChange(pageSize: number) {
  await router.push({
    name: 'family-browser',
    query: {
      keyword: keywordRef.value,
      categoryId: selectedKeysRef.value[0],
      page: currentRoute.query.page,
      pageSize: pageSize,
      order: currentRoute.query.order
    }
  })
}

async function handleKeywordSearch() {
  if (keywordRef.value) {
    handleTags({
      type: 'success',
      value: keywordRef.value
    })
    await router.push({
      name: 'family-browser',
      query: {
        keyword: keywordRef.value,
        categoryId: selectedKeysRef.value[0],
        page: currentRoute.query.page,
        pageSize: currentRoute.query.pageSize,
        order: currentRoute.query.order
      }
    })
  }
}
async function handlePageChange(newPage: number) {
  await router.push({
    name: 'family-browser',
    query: {
      keyword: currentRoute.query.keyword,
      categoryId: selectedKeysRef.value[0],
      page: newPage,
      pageSize: currentRoute.query.pageSize,
      order: currentRoute.query.order
    }
  })
}
async function handleOrderByChanged(value: string) {
  await router.push({
    name: 'family-browser',
    query: {
      keyword: currentRoute.query.keyword,
      categoryId: selectedKeysRef.value[0],
      page: currentRoute.query.page,
      pageSize: currentRoute.query.pageSize,
      order: value
    }
  })
}
async function getFamiliesPage(
  keyword?: string,
  categoryId?: string,
  pageIndex: number = 1,
  pageSize: number = 30,
  orderBy: string = 'default'
) {
  try {
    loadingBar.start()
    loading.value = true
    const httpResponse = await getFamiliesPagePromise(
      keyword,
      categoryId,
      pageIndex,
      pageSize,
      orderBy
    )
    if (httpResponse.succeed) {
      familiesRef.value = httpResponse.response.data
    } else {
      throw new Error(httpResponse.message)
    }
  } catch (error: any) {
    message.error(error.message)
  } finally {
    loadingBar.finish()
    loading.value = false
  }
}

onMounted(async () => {
  await getCategories()
  const selectedCategory = findItemByCondition(categoriesRef.value, (fc) => fc.id === categoryId)
  const tags: TagInfo[] = [
    {
      type: 'info',
      value: selectedCategory?.name as string
    },
    {
      type: 'success',
      value: keyword as string
    }
  ]
  for (let index = 0; index < tags.length; index++) {
    const tag = tags[index]
    handleTags(tag)
  }
})

watch(
  () => currentRoute.fullPath,
  async () => {
    await getFamiliesPage(
      currentRoute.query.keyword as string,
      currentRoute.query.categoryId as string,
      Number(currentRoute.query.page),
      Number(currentRoute.query.pageSize),
      currentRoute.query.orderBy as string
    )
  },
  { immediate: true }
)
</script>
