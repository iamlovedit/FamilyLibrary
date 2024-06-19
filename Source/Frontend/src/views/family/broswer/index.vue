<template>
  <n-flex>
    <div class="flex-1">
      <n-tree  expand-on-click :data="categoriesRef" key-field="id" label-field="name">

      </n-tree>
    </div>

    <n-felx>

    </n-felx>
  </n-flex>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { type FamilyCategory, getFamilyCategories } from '@/api/family'
import { useMessage, useLoadingBar } from 'naive-ui'


const message = useMessage()
const loadingBar = useLoadingBar()
const categoriesRef = ref<FamilyCategory[]>([])

async function getCategories() {
  try {
    loadingBar.start()
    const httpResponse = await getFamilyCategories()
    if (httpResponse.succeed) {
      categoriesRef.value = httpResponse.response
      console.log(categoriesRef.value)
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
})

</script>
