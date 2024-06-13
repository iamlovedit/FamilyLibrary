import { createRouter, createWebHistory } from 'vue-router'
import familyRoutes from './modules/family'
import exceptionRoutes from './modules/exception'
import packageRoutes from './modules/packages'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/login',
      name: 'login',
      component: () => import('@/views/login/index.vue'),
      meta: {
        title: '登录',
        requiresAuth: false
      }
    },
    {
      path: '/',
      name: 'home',
      component: () => import('@/views/home/index.vue'),
      meta: {
        title: '首页',
        requiresAuth: false
      }
    },
    ...familyRoutes,
    ...exceptionRoutes,
    ...packageRoutes
  ],
  scrollBehavior: () => ({ left: 0, top: 0 })
})

export default router
