import { RouteObject } from 'react-router-dom'
import { lazy, Suspense } from 'react'

const Loading = lazy(() => import('@/components/Loading'))
const DynamoHome = lazy(() => import('@/views/dynamo/index'))
const DynamoBroswer = lazy(() => import('@/views/dynamo/broswer/index'))
const DynamoDetail = lazy(() => import('@/views/dynamo/detail/index'))
const dynamoRoutes: RouteObject[] = [
  {
    path: '/dynamo',
    element:
      <Suspense fallback={<Loading />}>
        <DynamoHome />
      </Suspense>,
    children: [
      {
        path: 'broswer',
        element:
          <Suspense fallback={<Loading />}>
            <DynamoBroswer />
          </Suspense>,
      },
      {
        path: 'detail',
        element:
          <Suspense fallback={<Loading />}>
            <DynamoDetail />
          </Suspense>
      }
    ]
  }
]

export default dynamoRoutes
