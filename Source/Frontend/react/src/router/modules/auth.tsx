import { RouteObject } from 'react-router-dom'
import { lazy } from 'react'

const Authentication = lazy(() => import('@/views/auth'))
const Gitee = lazy(() => import('@/views/auth/gitee'))
const Login = lazy(() => import('@/views/login'))
const authRoutes: RouteObject[] = [
    {
        path: '/auth',
        element: <Authentication />,
        children: [
            {
                path: 'gitee',
                element: <Gitee />
            }
        ],
    },
    {
        path: '/login',
        element: <Login />
    }
]

export default authRoutes;