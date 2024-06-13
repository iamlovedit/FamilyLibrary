import type { LocationQueryRaw, Router } from 'vue-router'
import { useUser } from '@/stores/modules/user'

export function createRouterGuards(router: Router) {
  const userStore = useUser()

  router.beforeEach((to: any, from: any, next: any) => {
    window.document.title = to.meta.title
    if (!to.meta.requiresAuth) {
      next()
    }
  })
}
