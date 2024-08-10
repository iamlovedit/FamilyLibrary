import { RouteObject } from 'react-router-dom'
import { lazy, Suspense } from 'react'

const Loading = lazy(() => import('@/components/Loading'))
const FamilyHome = lazy(() => import('@/views/family'))
const FamilyBroswer = lazy(() => import('@/views/family/broswer'))
const FamilyDetail = lazy(() => import('@/views/family/detail'))
const familyRoutes: RouteObject[] = [
    {
        path: 'family',
        element:
            <Suspense fallback={<Loading />}>
                <FamilyHome />
            </Suspense>,
        children: [
            {
                path: 'broswer',
                element:
                    <Suspense fallback={<Loading />}>
                        <FamilyBroswer />
                    </Suspense>
            },
            {
                path: 'detail:/id',
                element:
                    <Suspense fallback={<Loading />}>
                        <FamilyDetail />
                    </Suspense>
            }
        ]
    }
]

export default familyRoutes
