import { RouteObject } from 'react-router-dom'
import { lazy, Suspense } from 'react'

const Loading = lazy(() => import('@/components/Loading'))
const Authentication = lazy(() => import('@/views/auth'))
const Gitee = lazy(() => import('@/views/auth/gitee'))
const Login = lazy(() => import('@/views/login'))
const authRoutes: RouteObject[] = [
    {
        path: '/auth',
        element:
            <Suspense fallback={<Loading />}>
                <Authentication />
            </Suspense>,
        children: [
            {
                path: 'gitee',
                element:
                    <Suspense fallback={<Loading />}>
                        <Gitee />
                    </Suspense>
            }
        ],
    },
    {
        path: '/login',
        element:
            <Suspense fallback={<Loading />}>
                <Login />
            </Suspense>
    }
]

export default authRoutes;