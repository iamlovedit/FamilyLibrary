<template>
  <div>
    {{ code }}
  </div>
</template>

<script setup lang="ts">
import { useRoute } from 'vue-router'
import request from '@/utils/request'
const route = useRoute()

const code = route.query.code
request({
  url: `/auth/gitee?code=${code}`,
  method: 'GET'
})
  .then((res) => {
    console.log(res)
  })
  .catch((error) => {
    alert(error.message)
  })
</script>

<style scoped></style>
