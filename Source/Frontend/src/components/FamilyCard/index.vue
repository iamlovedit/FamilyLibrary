<template>
  <div class="family-container" @click="$emit('handleDetailClick')">
    <n-image
      :src="cover"
      width="200"
      height="160"
      lazy
      :preview-disabled="true"
      fallback-src="https://07akioni.oss-cn-beijing.aliyuncs.com/07akioni.jpeg"
    />
    <n-flex justify="space-around">
      <n-tag type="info">
        {{ category }}
      </n-tag>
      <n-tag type="info"> rfa </n-tag>
    </n-flex>

    <n-button
      tag="a"
      text
      type="primary"
      target="_blank"
      class="mb-2"
      @click="$emit('handleDetailClick')"
    >
      {{ name }}
    </n-button>
  </div>
</template>

<script setup lang="ts">
import { computed } from 'vue'
import { useThemeVars } from 'naive-ui'

interface Props {
  cover: string
  name: string
  category: string
}
const emits = defineEmits<{
  (e: 'handleDetailClick'): void
}>()
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
  box-sizing: border-box;
  padding: 12px;
  justify-content: space-between;
  max-width: 200px;
  width: 200px;
  height: 260px;
  max-height: 260px;
  border: 1px solid v-bind(borderColor);
  border-radius: v-bind(borderRadius);
}
.family-container:hover {
  cursor: pointer;
}
</style>
