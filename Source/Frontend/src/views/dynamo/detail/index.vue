<template>
  <div>{{ descrptionRef }}</div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRoute } from 'vue-router';
import { useMessage, useLoadingBar } from 'naive-ui';
import { getPackageVersionPageAsync, getPackageDescriptionAsync, type PackageVersionDTO } from '@/api/dynamo';

const currentRoute = useRoute()
const message = useMessage()
const loadingBar = useLoadingBar()
const versionsRef = ref<PackageVersionDTO[]>([]);
const descrptionRef = ref<string>()
onMounted(async () => {
  try {
    loadingBar.start()
    const id = currentRoute.params.id as string;
    const [versionResponse, descriptionResponse] = await Promise.all([getPackageVersionPageAsync(id), getPackageDescriptionAsync(id)])
    console.log(versionResponse, descriptionResponse)
    if (versionResponse.succeed && descriptionResponse.succeed) {
      versionsRef.value = versionResponse.response.data;
      descrptionRef.value = descriptionResponse.response
    }
    else {
      const errorMessage = versionResponse.message || descriptionResponse.message || '未知错误';
      throw new Error(errorMessage)
    }
  } catch (error: any) {
    message.error(error.message)
  }
  finally {
    loadingBar.finish()
  }
})
</script>

<style scoped></style>
