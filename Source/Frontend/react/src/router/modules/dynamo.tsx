import { RouteObject } from 'react-router-dom'
import React, { Suspense, lazy } from 'react'

const DynamoHome = lazy(() => import('@/views/dynamo/index'))

const dynamoRoutes: RouteObject[] = [
  {
    path: '/dynamo',
    element: <DynamoHome />
  }
]

export default dynamoRoutes
