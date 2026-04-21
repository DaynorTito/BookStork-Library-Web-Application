import { renderHook, act } from '@testing-library/react'
import { describe, it, expect } from 'vitest'
import { useLocalStorage } from './useLocalStorage'

describe('useLocalStorage', () => {
  it('returns the initial value when nothing is stored', () => {
    const { result } = renderHook(() => useLocalStorage('test-key', 'default'))
    expect(result.current[0]).toBe('default')
  })

  it('reads an existing value from localStorage', () => {
    localStorage.setItem('existing-key', JSON.stringify('stored-value'))
    const { result } = renderHook(() => useLocalStorage('existing-key', 'default'))
    expect(result.current[0]).toBe('stored-value')
  })

  it('persists a new value to localStorage', () => {
    const { result } = renderHook(() => useLocalStorage('write-key', 0))

    act(() => {
      result.current[1](99)
    })

    expect(result.current[0]).toBe(99)
    expect(JSON.parse(localStorage.getItem('write-key')!)).toBe(99)
  })

  it('supports functional updater form', () => {
    const { result } = renderHook(() => useLocalStorage('counter', 5))

    act(() => {
      result.current[1]((prev) => prev + 1)
    })

    expect(result.current[0]).toBe(6)
    expect(JSON.parse(localStorage.getItem('counter')!)).toBe(6)
  })

  it('removes the value from localStorage and resets to initial', () => {
    const { result } = renderHook(() => useLocalStorage('removable', 'start'))

    act(() => {
      result.current[1]('changed')
    })
    expect(result.current[0]).toBe('changed')

    act(() => {
      result.current[2]() // removeValue
    })

    expect(result.current[0]).toBe('start')
    expect(localStorage.getItem('removable')).toBeNull()
  })

  it('handles object values correctly', () => {
    const initial = { name: 'Alice', age: 30 }
    const { result } = renderHook(() => useLocalStorage('obj-key', initial))

    act(() => {
      result.current[1]({ name: 'Bob', age: 25 })
    })

    expect(result.current[0]).toEqual({ name: 'Bob', age: 25 })
  })

  it('falls back to initialValue when localStorage contains invalid JSON', () => {
    localStorage.setItem('bad-json', '{not valid json')
    const { result } = renderHook(() => useLocalStorage('bad-json', 'fallback'))
    expect(result.current[0]).toBe('fallback')
  })
})
