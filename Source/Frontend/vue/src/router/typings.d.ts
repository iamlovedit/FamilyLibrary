import 'vue-router'

declare module 'vue-router' {
  interface RouteMeta {
    requiresAuth?: boolean
    ignoreCache?: boolean
    icon?: any
  }
}
