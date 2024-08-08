import { lazy } from 'react'
import { RouteObject, useRoutes } from 'react-router-dom'
import dynamoRoutes from './modules/dynamo'
import familyRoutes from './modules/family'
import excptionRoutes from './modules/exceptions'
import authRoutes from './modules/auth'

const Home = lazy(() => import('@/views/home/index'))

const routes: RouteObject[] = [
  {
    path: '/',
    element: <Home />
  },
  ...dynamoRoutes,
  ...familyRoutes,
  ...excptionRoutes,
  ...authRoutes
]

const MainRoutes = () => {
  return useRoutes(routes)
}

export default MainRoutes;
