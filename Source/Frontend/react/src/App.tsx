import { RouterProvider } from 'react-router-dom'
import router from '@/router'
import { QueryClient, QueryClientProvider } from 'react-query'

const queryClient = new QueryClient()

const App: React.FC = () => {
  return (
    <QueryClientProvider client={queryClient}>
      <RouterProvider router={router} />
    </QueryClientProvider>
  )
}
export default App
