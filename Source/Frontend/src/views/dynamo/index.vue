<template>
  <n-flex vertical ustify="space-between" class="p-5">
    <n-input-group>
      <n-input
        :style="{ width: '50%' }"
        show-count
        :maxlength="12"
        placeholder="搜索节点包"
        v-model:value="keyword"
      />
      <n-button type="primary" ghost @click="handleSearch"> 搜索 </n-button>
    </n-input-group>
    <div class="flex-1">
      <router-view />
    </div>
  </n-flex>
</template>
<script setup lang="ts">
import { ref, watch } from 'vue'
import { useRoute, useRouter } from 'vue-router'

const route = useRoute()
const router = useRouter()

const { query } = route

const keyword = ref<string>(query.keyword as any as string)
function handleSearch(e: Event) {
  e.preventDefault()
  if (keyword.value) {
    try {
      console.log(keyword.value)
      router.push({
        name: 'package-broswer',
        query: {
          keyword: keyword.value,
          page: 1,
          pageSize: 30,
          orderBy: 'default'
        }
      })
    } catch (error: any) {
      console.log(error)
    }
  }
}

watch(
  () => route.fullPath,
  () => {
    keyword.value = route.query.keyword as string
  }
)
</script>
