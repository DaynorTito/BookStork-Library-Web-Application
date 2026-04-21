import React from 'react'
import { Provider } from 'react-redux'
import { BrowserRouter } from 'react-router-dom'
import { store } from './core/store/store'
import { ToastProvider } from './shared/context/ToastContext'
import AppRoutes from './core/routes/AppRoutes'
import './App.css'

function App() {
  return (
    <Provider store={store}>
      <BrowserRouter>
        <ToastProvider>
          <AppRoutes />
        </ToastProvider>
      </BrowserRouter>
    </Provider>
  )
}

export default App
