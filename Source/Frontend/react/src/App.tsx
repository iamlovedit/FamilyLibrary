import './App.css'
import { BrowserRouter } from 'react-router-dom';
import MainRoutes from '@/router';

function App() {
  return (
    <BrowserRouter>
      <MainRoutes />
    </BrowserRouter>
  )
}
export default App
