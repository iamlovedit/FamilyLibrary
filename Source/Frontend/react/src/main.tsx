import 'virtual:uno.css'
import React from 'react'
import ReactDOM from 'react-dom/client'
import App from './App.tsx'
import './index.css'

const rootContainer = ReactDOM.createRoot(document.getElementById('root')!);
rootContainer.render(
  <React.StrictMode>
    <App />
  </React.StrictMode>,
)
