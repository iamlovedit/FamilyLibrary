import { BrowserRouter, Outlet } from 'react-router-dom';
import MainRoutes from '@/router';
import { Layout, Menu } from 'antd';
import type { MenuProps } from 'antd';
type MenuItem = Required<MenuProps>['items'][number];

const items: MenuItem[] = [
    {
        label: '首页',
        key: 'home'
    },
    {
        label: '节点包',
        key: 'dynamo'
    },
    {
        label: '族库',
        key: 'family'
    }
]

const { Header, Content, Footer } = Layout;
function Home() {
    return (
        <Layout className='h-full w-full flex flex-col flex-nowrap'>
            <Header >
                <Menu items={items} mode='horizontal' theme="dark" />
            </Header>
            <Content>
                <Outlet />
            </Content>
            <Footer className='text-align-center'>
                Ant Design ©{new Date().getFullYear()} Created by Ant UED
            </Footer>
        </Layout>
    )
}

export default Home