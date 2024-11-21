import { BrowserRouter } from 'react-router-dom';
import MainRoutes from '@/router';

const App: React.FC = () => {
  return (
    <BrowserRouter>
      <MainRoutes />
    </BrowserRouter>
  )
}
export default App
