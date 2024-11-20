import 'virtual:uno.css'
import { NextUIProvider } from "@nextui-org/react";
import React from 'react'
import ReactDOM from 'react-dom/client'
import App from './App.tsx'
import './index.css'
import { Provider } from 'react-redux'
import store from '@/stores'
const rootContainer = ReactDOM.createRoot(document.getElementById('root')!);
rootContainer.render(
  <React.StrictMode>
    <NextUIProvider>
      <Provider store={store}>
        <App />
      </Provider>
    </NextUIProvider>
  </React.StrictMode>,
)
