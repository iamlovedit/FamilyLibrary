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
          pageSize: 30
        }
      }
    },
    children: [
      {
        path: 'broswer',
        name: 'package-broswer',
        component: () => import('@/views/dynamo/broswer/index.vue'),
        props: (route: any) => ({
          keyword: route.query.keyword || '',
          page: parseInt(route.query.page) || 1,
          pageSize: parseInt(route.query.pageSize) || 30,
          orderBy: route.query.order || ''
        }),
        beforeEnter: (to: any, from: any, next) => {
          const page = parseInt(to.query.page)
          const pageSize = parseInt(to.query.pageSize)
          if (Number.isInteger(page) && Number.isInteger(pageSize)) {
            next()
          } else {
            next('/404')
          }
        }
      },
      {
        path: 'detail/:id',
        name: 'package-detail',
        component: () => import('@/views/dynamo/detail/index.vue')
      }
    ]
  }
]

export default packageRoutes
