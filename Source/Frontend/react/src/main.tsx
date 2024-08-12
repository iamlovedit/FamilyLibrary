import 'virtual:uno.css'
import React from 'react'
import ReactDOM from 'react-dom/client'
import App from './App.tsx'
import './index.css'
import { Provider } from 'react-redux'
import store from '@/stores'
const rootContainer = ReactDOM.createRoot(document.getElementById('root')!);
rootContainer.render(
  <React.StrictMode>
    <Provider store={store}>
      <App />
    </Provider>
  </React.StrictMode>,
)
