import { RouteObject } from 'react-router-dom'
import React, { Suspense, lazy } from 'react'

const FamilyHome = lazy(() => import('@/views/family'))
const familyRoutes: RouteObject[] = [
    {
        path: '/family',
        element: <FamilyHome />
    }
]

export default familyRoutes
