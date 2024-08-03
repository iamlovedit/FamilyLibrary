import { lazy } from 'react'
import { RouteObject, useRoutes } from 'react-router-dom'
import dynamoRoutes from './modules/dynamo'
import familyRoutes from './modules/family'

const Home = lazy(() => import('@/views/home/index'))

const routes: RouteObject[] = [
  {
    path: '/',
    element: <Home />
  },
  ...dynamoRoutes,
  ...familyRoutes
]

const MainRoutes = () => {
  return useRoutes(routes)
}

export default MainRoutes;
