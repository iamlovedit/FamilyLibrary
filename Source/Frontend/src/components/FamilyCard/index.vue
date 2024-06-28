<template>
  <div class="family-container">
    <n-image
      :src="cover"
      width="200"
      height="160"
      lazy
      :preview-disabled="true"
      fallback-src="https://07akioni.oss-cn-beijing.aliyuncs.com/07akioni.jpeg"
    />
    <n-button tag="a" text type="primary" target="_blank">
      {{ props.name }}
    </n-button>
  </div>
</template>

<script setup lang="ts">
import { computed } from 'vue'
import { useThemeVars } from 'naive-ui'

interface Props {
  cover: string
  name: string
}

const props = defineProps<Props>()

const themeVars = useThemeVars()
const borderColor = computed(() => {
  return themeVars.value.borderColor
})

const borderRadius = computed(() => {
  return themeVars.value.borderRadius
})
</script>

<style scoped less>
.family-container {
  display: flex;
  flex-flow: column;
  flex-wrap: nowrap;
  justify-content: space-between;
  max-width: 200px;
  width: 200px;
  height: 260px;
  max-height: 260px;
  border: 1px solid v-bind(borderColor);
  border-radius: v-bind(borderRadius);
}
</style>
