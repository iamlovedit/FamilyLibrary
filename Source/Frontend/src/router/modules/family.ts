import type { RouteRecordRaw, Route } from 'vue-router'

const familyRoutes: RouteRecordRaw[] = [
  {
    path: '/family',
    name: 'family',
    component: () => import('@/views/family/index.vue'),
    redirect() {
      return {
        name: 'family-browser',
        query: {
          page: 1,
          pageSize: 30
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
        component: () => import('@/views/family/broswer/index.vue'),
        props: (route: Route) => ({
          keyword: route.query.keyword || '',
          categoryId: route.query.categoryId || '',
          order: route.query.order || '',
          page: parseInt(route.query.page) || 1,
          pageSize: parseInt(route.query.pageSize) || 30
        }),
        beforeEnter: (to: Route, from: Route, next) => {
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
        path: 'detail/:id(\\d{19})',
        name: 'family-detail',
        component: () => import('@/views/family/detail/index.vue')
      }
    ]
  }
]

export default familyRoutes
