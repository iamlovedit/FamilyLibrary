import './App.css'
import { BrowserRouter } from 'react-router-dom';
import MainRoutes from '@/router';
import { ConfigProvider, theme } from "antd";

function App() {
  return (
    <ConfigProvider theme={{ algorithm: theme.darkAlgorithm }}>
      <BrowserRouter>
        <MainRoutes />
      </BrowserRouter>
    </ConfigProvider>
  )
}
export default App
