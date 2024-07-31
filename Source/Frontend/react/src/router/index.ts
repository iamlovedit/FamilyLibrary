import React from 'react'
import * as ReactDOM from 'react-dom'
import { createBrowserRouter, RouterProvider, RouteObject } from 'react-router-dom'
import dynamoRoutes from './modules/dynamo'
import familyRoutes from './modules/family'

const routes: RouteObject[] = [
  {
    path: '/'
  }
]

export default routes
