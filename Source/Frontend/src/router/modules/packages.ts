import type { RouteRecordRaw } from 'vue-router'

const packageRoutes: RouteRecordRaw[] = [
  {
    path: '/package',
    name: 'package-home',
    component: () => import('@/views/dynamo/index.vue'),
    meta: {
      title: '节点包',
      requiresAuth: false
    }
  }
]

export default packageRoutes
