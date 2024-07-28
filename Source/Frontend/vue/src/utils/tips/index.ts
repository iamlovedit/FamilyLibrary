import { createDiscreteApi, darkTheme, lightTheme } from 'naive-ui'
import type { ConfigProviderProps } from 'naive-ui'
import { computed, ref } from 'vue'

import { getLocalTheme } from '@/stores/modules/app/helper'

const themeRef = ref<'light' | 'dark'>(getLocalTheme())

const configProviderPropsRef: any = computed<ConfigProviderProps>(() => ({
  theme: themeRef.value === 'light' ? lightTheme : darkTheme
}))

const { message, notification, dialog, loadingBar } = createDiscreteApi(
  ['message', 'dialog', 'notification', 'loadingBar'],
  {
    configProviderProps: configProviderPropsRef
  }
)

export {
  dialog as Dialog,
  loadingBar as LoadingBar,
  message as Message,
  notification as Notification,
  themeRef
}
