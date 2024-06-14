import type { RouteRecordRaw } from 'vue-router'

const packageRoutes: RouteRecordRaw[] = [
  {
    path: '/package',
    name: 'package',
    component: () => import('@/views/dynamo/index.vue'),
    meta: {
      title: '节点包',
      requiresAuth: false
    },
    redirect() {
      return {
        name: 'package-broswer',
        query: {
          page: 1,
          pageSize: 30,
          orderBy: 'default'
        }
      }
    },
    children: [
      {
        path: 'broswer',
        name: 'package-broswer',
        component: () => import('@/views/dynamo/broswer/index.vue')
      },
      {
        path: 'detail',
        name: 'package-detail',
        component: () => import('@/views/dynamo/detail/index.vue')
      }
    ]
  }
]

export default packageRoutes
