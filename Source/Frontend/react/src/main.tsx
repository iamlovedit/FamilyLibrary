import { NextUIProvider } from '@nextui-org/react'
import { ThemeProvider as NextThemesProvider } from 'next-themes'
import React from 'react'
import ReactDOM from 'react-dom/client'
import { Toaster } from 'sonner'

import App from './App.tsx'
import './index.css'
const rootContainer = ReactDOM.createRoot(document.getElementById('root')!)
rootContainer.render(
  <React.StrictMode>
    <NextUIProvider>
      <NextThemesProvider attribute="class" defaultTheme="dark">
        <div className="text-foreground bg-background h-dvh w-full">
          <Toaster position="top-center" />
          <App />
        </div>
      </NextThemesProvider>
    </NextUIProvider>
  </React.StrictMode>
)
