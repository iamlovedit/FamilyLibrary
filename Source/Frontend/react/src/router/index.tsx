import { lazy } from 'react'
import { RouteObject, useRoutes } from 'react-router-dom'
import dynamoRoutes from './modules/dynamo'
import familyRoutes from './modules/family'
import excptionRoutes from './modules/exceptions'

const Home = lazy(() => import('@/views/home/index'))

const routes: RouteObject[] = [
  {
    path: '/',
    element: <Home />
  },
  ...dynamoRoutes,
  ...familyRoutes,
  ...excptionRoutes
]

const MainRoutes = () => {
  return useRoutes(routes)
}

export default MainRoutes;
