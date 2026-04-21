import React, { memo } from 'react'
import { useTheme } from '@/shared/hooks/useTheme'
import { Sun, Moon } from 'lucide-react'
import clsx from 'clsx'

const ThemeToggle = memo(function ThemeToggle({ className }: { className?: string }) {
  const [theme, toggleTheme] = useTheme()

  return (
    <button
      onClick={toggleTheme}
      aria-label={`Switch to ${theme === 'light' ? 'dark' : 'light'} mode`}
      className={clsx(
        'relative w-12 h-6 rounded-full transition-colors focus:outline-none focus:ring-2 focus:ring-primary-500 focus:ring-offset-2',
        theme === 'dark' ? 'bg-primary-700' : 'bg-gray-300',
        className,
      )}
    >
      <span
        className={clsx(
          'absolute top-0.5 left-0.5 w-5 h-5 rounded-full shadow transition-transform flex items-center justify-center bg-white',
          theme === 'dark' ? 'translate-x-6' : 'translate-x-0',
        )}
      >
        {theme === 'dark'
          ? <Moon size={11} className="text-gray-600" />
          : <Sun size={11} className="text-yellow-500" />
        }
      </span>
    </button>
  )
})

export default ThemeToggle
