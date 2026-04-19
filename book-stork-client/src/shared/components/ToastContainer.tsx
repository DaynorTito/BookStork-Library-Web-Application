import React, { memo } from 'react'
import { CheckCircle, XCircle, Info, AlertTriangle, X } from 'lucide-react'
import clsx from 'clsx'
import type { Toast } from '@/shared/hooks/useToast'

interface ToastContainerProps {
  toasts: Toast[]
  onRemove: (id: string) => void
}

const IconMap: Record<Toast['type'], React.ElementType> = {
  success: CheckCircle,
  error: XCircle,
  info: Info,
  warning: AlertTriangle,
}

const colorMap: Record<Toast['type'], string> = {
  success: 'text-emerald-400',
  error: 'text-red-400',
  info: 'text-blue-400',
  warning: 'text-amber-400',
}

const ToastContainer = memo(function ToastContainer({ toasts, onRemove }: ToastContainerProps) {
  return (
    <div className="fixed top-4 right-4 z-[100] flex flex-col gap-2 w-80 pointer-events-none">
      {toasts.map((t) => {
        const Icon = IconMap[t.type]
        return (
          <div
            key={t.id}
            className="pointer-events-auto flex items-start gap-3 rounded-lg p-4 shadow-lg text-white bg-gray-900 dark:bg-dark-card border border-white/10"
          >
            <Icon size={18} className={clsx('flex-shrink-0 mt-0.5', colorMap[t.type])} />
            <p className="text-sm flex-1 leading-snug">{t.message}</p>
            <button
              onClick={() => onRemove(t.id)}
              className="text-gray-400 hover:text-white flex-shrink-0"
              aria-label="Dismiss"
            >
              <X size={14} />
            </button>
          </div>
        )
      })}
    </div>
  )
})

export default ToastContainer
