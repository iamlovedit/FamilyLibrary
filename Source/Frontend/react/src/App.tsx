import '@/App.css'
import { BrowserRouter } from 'react-router-dom';
import MainRoutes from '@/router';
import { ConfigProvider, theme } from 'antd';

const App: React.FC = () => {
  return (
    <ConfigProvider theme={{ algorithm: theme.darkAlgorithm }}>
      <BrowserRouter>
        <MainRoutes />
      </BrowserRouter>
    </ConfigProvider>
  )
}
export default App
