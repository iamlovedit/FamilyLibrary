import { useOsTheme } from 'naive-ui'
import { ss } from '@/utils/storage'

const osThemeRef = useOsTheme()

const THEMEKEY = 'key_theme'

export function getLocalTheme() {
  const theme = ss.get(THEMEKEY) || osThemeRef.value
  return theme
}

export function setLocalTheme(theme: string) {
  ss.set(THEMEKEY, theme)
}
