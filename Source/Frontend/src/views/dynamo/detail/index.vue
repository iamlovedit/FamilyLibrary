<template>
  <div>package detail</div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRoute } from 'vue-router';
import { useMessage, useLoadingBar } from 'naive-ui';
import { getPackageVersionPageAsync, type PackageVersionDTO } from '@/api/dynamo';

const currentRoute = useRoute()
const message = useMessage()
const loadingBar = useLoadingBar()
const versionsRef = ref<PackageVersionDTO[]>([]);

onMounted(async () => {
  try {
    loadingBar.start()
    var httpResponse = await getPackageVersionPageAsync(currentRoute.params.id as string);
    console.log(httpResponse)
    if (httpResponse.succeed) {
      versionsRef.value = httpResponse.response.data;
    }
    else {
      throw new Error(httpResponse.message)
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
