<template>
  <n-flex vertical ustify="space-between">
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
    <router-view />
  </n-flex>
</template>

<script setup lang="ts">
import { ref, computed, watch } from 'vue'
import { useRoute, useRouter } from 'vue-router'

const route = useRoute()
const router = useRouter()

const keyword = ref<string>()

const page = computed(() => route.query.page as any as number)

const pageSize = computed(() => route.query.pageSize as any as number)

function handleSearch(e: Event) {
  e.preventDefault()
  if (keyword.value) {
    try {
      console.log(keyword.value)
      router.push({
        name: 'package-broswer',
        query: {
          keyword: keyword.value,
          page: page.value,
          pageSize: pageSize.value
        }
      })
    } catch (error: any) {
      console.log(error)
    }
  }
}

watch(
  () => route.query,
  () => {
    console.log(route.fullPath)
  }
)
</script>

<style scoped></style>
