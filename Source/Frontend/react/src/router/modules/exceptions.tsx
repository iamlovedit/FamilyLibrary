import { RouteObject } from 'react-router-dom'
import { lazy } from 'react'

const NotFound = lazy(() => import('@/views/exceptions/404'))
const excptionRoutes: RouteObject[] = [
    {
        path: '/404',
        element: <NotFound />
    }
]

export default excptionRoutes