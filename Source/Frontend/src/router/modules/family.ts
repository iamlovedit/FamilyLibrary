import type { RouteRecordRaw } from 'vue-router'

const familyRoutes: RouteRecordRaw[] = [
  {
    path: '/family',
    name: 'family-home',
    component: () => import('@/views/family/index.vue'),
    meta: {
      title: '族库',
      requiresAuth: false
    }
  }
]

export default familyRoutes
