import type { RouteRecordRaw } from 'vue-router'

const packageRoutes: RouteRecordRaw[] = [
  {
    path: '/package',
    name: 'package-home',
    component: () => import('@/views/dynamo/index.vue'),
    meta: {
      title: '节点包首页',
      requiresAuth: false
    }
  }
]

export default packageRoutes
