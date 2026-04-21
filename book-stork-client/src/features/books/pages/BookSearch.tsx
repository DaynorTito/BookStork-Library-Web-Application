import React, { useState, useCallback } from 'react'
import { Search, X } from 'lucide-react'
import { useDebounce } from '@/shared/hooks/useDebounce'

interface BookSearchProps {
  onSearch: (keyword: string) => void
  placeholder?: string
}

export default function BookSearch({ onSearch, placeholder = 'Search books, authors…' }: BookSearchProps) {
  const [value, setValue] = useState('')
  const debounced = useDebounce(value, 400)

  const prevDebounced = React.useRef(debounced)
  React.useEffect(() => {
    if (debounced !== prevDebounced.current) {
      onSearch(debounced)
      prevDebounced.current = debounced
    }
  }, [debounced, onSearch])

  const handleClear = useCallback(() => {
    setValue('')
    onSearch('')
  }, [onSearch])

  return (
    <div className="relative w-full max-w-sm">
      <Search size={15} className="absolute left-3 top-1/2 -translate-y-1/2 text-gray-400 pointer-events-none" />
      <input
        type="search"
        value={value}
        onChange={(e) => setValue(e.target.value)}
        placeholder={placeholder}
        className="input-base pl-9 pr-8"
      />
      {value && (
        <button
          onClick={handleClear}
          className="absolute right-2 top-1/2 -translate-y-1/2 text-gray-400 hover:text-gray-600 dark:hover:text-gray-200"
          aria-label="Clear search"
        >
          <X size={14} />
        </button>
      )}
    </div>
  )
}
