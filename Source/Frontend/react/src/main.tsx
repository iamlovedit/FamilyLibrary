import { NextUIProvider } from "@nextui-org/react";
import React from 'react'
import ReactDOM from 'react-dom/client'
import App from './App.tsx'
import './index.css'
const rootContainer = ReactDOM.createRoot(document.getElementById('root')!);
rootContainer.render(
  <React.StrictMode>
    <NextUIProvider>
      <div className="dark text-foreground bg-background h-dvh w-full">
        <App />
      </div>
    </NextUIProvider>
  </React.StrictMode>,
)
