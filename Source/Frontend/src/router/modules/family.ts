import type { RouteRecordRaw } from 'vue-router'

const familyRoutes: RouteRecordRaw[] = [
  {
    path: '/family',
    name: 'family',
    component: () => import('@/views/family/index.vue'),
    redirect() {
        return{
          name:'family-browser',
          query:{
            page:1,
            pageSize:30
          }
        }
    },
    meta: {
      title: '族库',
      requiresAuth: false
    },
    children: [
      {
        path: 'broswer',
        name: 'family-browser',
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
