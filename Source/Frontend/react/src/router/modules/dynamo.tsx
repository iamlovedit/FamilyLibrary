import { RouteObject } from 'react-router-dom'
import { lazy } from 'react'

const DynamoHome = lazy(() => import('@/views/dynamo/index'))
const DynamoBroswer = lazy(() => import('@/views/dynamo/broswer/index'))
const dynamoRoutes: RouteObject[] = [
  {
    path: '/dynamo',
    element: <DynamoHome />,
    children: [
      {
        path: 'broswer',
        element: <DynamoBroswer />
      }
    ]
  }
]

export default dynamoRoutes
