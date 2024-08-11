import { Outlet, useNavigate, useLocation } from 'react-router-dom';
import { Layout, Menu } from 'antd';
import type { MenuProps } from 'antd';
import { useState } from 'react';

type MenuItem = Required<MenuProps>['items'][number];

const items: MenuItem[] = [
    {
        label: '首页',
        key: '/'
    },
    {
        label: '节点包',
        key: '/dynamo'
    },
    {
        label: '族库',
        key: '/family'
    }
]
const { Header, Content, Footer } = Layout;
function MainLayout() {
    const { pathname } = useLocation();
    const navigate = useNavigate();
    const [currentItem, setCurrentItem] = useState(pathname)
    const onClick: MenuProps['onClick'] = (e) => {
        setCurrentItem(e.key);
        navigate(e.key)
    }
    return (
        <Layout className='h-full w-full flex flex-col flex-nowrap'>
            <Header>
                <Menu items={items}
                    onClick={onClick}
                    selectedKeys={[currentItem]}
                    mode='horizontal' theme="dark" />
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

export default MainLayout

