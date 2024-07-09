<template>
  <n-flex class="flex-1">
    <n-card class="w-300px min-w-200px">
      <n-tree
        :data="categoriesRef"
        key-field="id"
        label-field="name"
        :default-expanded-keys="expandedKeysRef"
        v-model:selected-keys="selectedKeysRef"
        @update:selected-keys="handleSelectedChanged"
      >
      </n-tree>
    </n-card>
    <n-card class="flex-1">
      <n-flex vertical class="gap-8 h-full" justify="space-between">
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
          <n-input :style="{ width: '50%' }" v-model:value="keywordRef" clearable />
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
            :category="family.category.name"
            :downloads="family.downloads"
            :stars="family.stars"
            cover="https://gw.alipayobjects.com/zos/antfincdn/aPkFc8Sj7n/method-draw-image.svg"
            @get-detail="() => handleGetDetail(family.id)"
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
    </n-card>
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
  id?: string
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
const expandedKeysRef = ref<string[]>([])
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
    value: 'stars',
    label: '收藏'
  },
  {
    value: 'createdDate',
    label: '上传时间'
  }
]

function findParentPath(items: FamilyCategory[], targetItem: FamilyCategory): string[] {
  const parents: string[] = []

  function findParentRecursive(
    currentItems: FamilyCategory[],
    currentItem: FamilyCategory
  ): boolean {
    for (const item of currentItems) {
      if (item === targetItem) {
        parents.unshift(item.id)
        return true
      }

      const found = findParentRecursive(item.children, currentItem)
      if (found) {
        parents.unshift(item.id)
        return true
      }
    }

    return false
  }
  findParentRecursive(items, targetItem)
  return parents
}

async function handleGetDetail(id: string) {
  await router.push({
    name: 'family-detail',
    params: {
      id: id
    }
  })
}

async function handleTagClosed(tag: TagInfo) {
  tagsRef.value = tagsRef.value.filter((t) => t.value !== tag.value)

  const commonQueryParams = {
    page: currentRoute.query.page,
    pageSize: currentRoute.query.pageSize,
    order: currentRoute.query.order
  }

  const routeToFamilyBrowser = async (query: Record<string, any>) => {
    await router.push({
      name: 'family-browser',
      query
    })
  }

  const handleNonEmptyTags = async () => {
    const firstTag = tagsRef.value[0]
    if (firstTag.type === 'info') {
      keywordRef.value = ''
      await routeToFamilyBrowser({
        categoryId: firstTag.id,
        ...commonQueryParams
      })
    } else {
      expandedKeysRef.value = []
      selectedKeysRef.value = []
      await routeToFamilyBrowser({
        keyword: firstTag.value,
        ...commonQueryParams
      })
    }
  }

  const handleEmptyTags = async () => {
    expandedKeysRef.value = []
    selectedKeysRef.value = []
    keywordRef.value = ''
    await routeToFamilyBrowser(commonQueryParams)
  }

  if (tagsRef.value.length > 0) {
    await handleNonEmptyTags()
  } else {
    await handleEmptyTags()
  }
}
async function handleSelectedChanged(
  keys: Array<string | number>,
  option: Array<TreeOption | null>
) {
  handleTags({
    type: 'info',
    id: keys[0] as string,
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
      orderBy: value
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
  if (selectedCategory) {
    expandedKeysRef.value = findParentPath(categoriesRef.value, selectedCategory)
    selectedKeysRef.value.push(selectedCategory.id)
  }
  const tags: TagInfo[] = [
    {
      type: 'info',
      id: selectedCategory?.id,
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
