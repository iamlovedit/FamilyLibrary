import type { RouteRecordRaw } from 'vue-router'

const familyRoutes: RouteRecordRaw[] = [
  {
    path: '/family',
    name: 'family',
    component: () => import('@/views/family/index.vue'),
    meta: {
      title: '族库',
      requiresAuth: false
    },
    children: [
      {
        path: 'broswer',
        name: 'family-broser',
        component: () => import('@/views/family/broswer/index.vue')
      },
      {
        path: 'detail',
        name: 'family-detail',
        component: () => import('@/views/family/detail/index.vue')
      }
    ]
  }
]

export default familyRoutes
