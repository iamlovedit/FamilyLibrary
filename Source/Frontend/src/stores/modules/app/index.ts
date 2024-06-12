import { ref } from 'vue'
import { defineStore } from 'pinia'
import { themeRef } from '@/utils/tips'
import { getLocalTheme, setLocalTheme } from './helper'

export const useAppStore = defineStore('app', () => {
  const theme = ref(getLocalTheme())
  function switchTheme() {
    theme.value = theme.value === 'light' ? 'dark' : 'light'
    themeRef.value = theme.value
    setLocalTheme(theme.value)
  }

  return {
    theme,
    switchTheme
  }
})
