<template>
  <n-flex vertical>
    <n-page-header :subtitle="familyRef?.name" @back="handleBack" class="w-full">
      <n-flex justify="space-around" size="large">
        <n-statistic label="类别" :value="`${familyRef?.category.name}`" />
        <n-statistic label="下载" :value="`${familyRef?.downloads} 次`" />
        <n-statistic label="点赞" :value="`${familyRef?.stars} 次`" />
        <n-statistic label="发布时间" :value="`${familyRef?.createdDate}`" />
      </n-flex>
    </n-page-header>
  </n-flex>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useMessage, useLoadingBar } from 'naive-ui'
import { useRoute, useRouter } from 'vue-router'
import { type FamilyDetail, getFamilyDetailPromise } from '@/api/family'

const router = useRouter()
const currentRoute = useRoute()
const message = useMessage()
const loadingBar = useLoadingBar()
const familyRef = ref<FamilyDetail>()

async function getFamilyDetailAsync(id: string) {
  try {
    loadingBar.start()
    const httpMessage = await getFamilyDetailPromise(id)
    if (httpMessage.succeed) {
      familyRef.value = httpMessage.response
      console.log(familyRef.value)
    } else {
      throw new Error(httpMessage.message)
    }
  } catch (error: any) {
    message.error(error.message)
  } finally {
    loadingBar.finish()
  }
}

function handleBack() {
  router.back()
}

onMounted(async () => {
  await getFamilyDetailAsync(currentRoute.params.id as string)
})
</script>
