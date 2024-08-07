import '@/App.css'
import { BrowserRouter } from 'react-router-dom';
import MainRoutes from '@/router';
import { Layout, Menu, ConfigProvider, theme } from 'antd';
import type { MenuProps } from 'antd';

type MenuItem = Required<MenuProps>['items'][number];

const { Header, Content, Footer } = Layout;

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

const App: React.FC = () => {
  return (
    <ConfigProvider theme={{ algorithm: theme.darkAlgorithm }}>
      <Layout className='h-full w-full flex flex-col flex-nowrap'>
        <Header >
          <Menu items={items} mode='horizontal' theme="dark" />
        </Header>
        <Content>
          <BrowserRouter>
            <MainRoutes />
          </BrowserRouter>
        </Content>
        <Footer className='text-align-center'>
          Ant Design ©{new Date().getFullYear()} Created by Ant UED
        </Footer>
      </Layout>
    </ConfigProvider>
  )
}
export default App
