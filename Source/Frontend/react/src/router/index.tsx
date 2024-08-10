import { lazy, Suspense } from 'react'
import { Navigate, RouteObject, useRoutes } from 'react-router-dom'
import dynamoRoutes from './modules/dynamo'
import familyRoutes from './modules/family'
import excptionRoutes from './modules/exceptions'
import authRoutes from './modules/auth'

const Home = lazy(() => import('@/views/home'))
const MainLayout = lazy(() => import('@/views/layout'))
const Loading = lazy(() => import('@/components/Loading'))

const routes: RouteObject[] = [
  {
    path: "*",
    element: <Navigate to='/404' />
  },
  {
    path: '',
    element:
      <Suspense fallback={<Loading />}>
        <MainLayout />
      </Suspense>,
    children: [
      {
        path: '/home',
        element:
          <Suspense fallback={<Loading />}>
            <Home />
          </Suspense>
      },
      ...dynamoRoutes,
      ...familyRoutes,
    ]
  },
  ...excptionRoutes,
  ...authRoutes
]

const MainRoutes = () => {
  return useRoutes(routes)
}

export default MainRoutes;
