import { RouteObject } from 'react-router-dom'
import { lazy } from 'react'

const FamilyHome = lazy(() => import('@/views/family'))
const FamilyBroswer = lazy(() => import('@/views/family/broswer'))
const FamilyDetail = lazy(() => import('@/views/family/detail'))
const familyRoutes: RouteObject[] = [
    {
        path: '/family',
        element: <FamilyHome />,
        children: [
            {
                path: 'broswer',
                element: <FamilyBroswer />
            },
            {
                path: 'detail',
                element: <FamilyDetail />
            }
        ]
    }
]

export default familyRoutes
