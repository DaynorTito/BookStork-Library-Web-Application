import { createContext, useContext, createElement, type ReactNode } from 'react'
import { useToast, type Toast } from '@/shared/hooks/useToast'
import ToastContainer from '@/shared/components/ToastContainer'

interface ToastContextValue {
  addToast: (message: string, type?: Toast['type']) => void
  removeToast: (id: string) => void
}

export const ToastContext = createContext<ToastContextValue | null>(null)

export function ToastProvider({ children }: { children: ReactNode }) {
  const { toasts, addToast, removeToast } = useToast()

  return createElement(
    ToastContext.Provider,
    { value: { addToast, removeToast } },
    children,
    createElement(ToastContainer, { toasts, onRemove: removeToast }),
  )
}

export function useToastContext(): ToastContextValue {
  const ctx = useContext(ToastContext)
  if (!ctx) throw new Error('useToastContext must be used within ToastProvider')
  return ctx
}
