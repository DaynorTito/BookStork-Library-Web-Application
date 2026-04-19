import { renderHook, act } from '@testing-library/react'
import { describe, it, expect, vi, beforeEach, afterEach } from 'vitest'
import { useToast } from './useToast'

describe('useToast', () => {
  beforeEach(() => {
    vi.useFakeTimers()
  })

  afterEach(() => {
    vi.useRealTimers()
  })

  it('starts with an empty toast list', () => {
    const { result } = renderHook(() => useToast())
    expect(result.current.toasts).toHaveLength(0)
  })

  it('adds a toast with the correct message and type', () => {
    const { result } = renderHook(() => useToast())

    act(() => {
      result.current.addToast('Hello!', 'success')
    })

    expect(result.current.toasts).toHaveLength(1)
    expect(result.current.toasts[0].message).toBe('Hello!')
    expect(result.current.toasts[0].type).toBe('success')
  })

  it('defaults to type "info" when no type is provided', () => {
    const { result } = renderHook(() => useToast())

    act(() => {
      result.current.addToast('Info message')
    })

    expect(result.current.toasts[0].type).toBe('info')
  })

  it('auto-removes a toast after 4 seconds', () => {
    const { result } = renderHook(() => useToast())

    act(() => {
      result.current.addToast('Temporary', 'warning')
    })

    expect(result.current.toasts).toHaveLength(1)

    act(() => {
      vi.advanceTimersByTime(4000)
    })

    expect(result.current.toasts).toHaveLength(0)
  })

  it('does not remove the toast before 4 seconds', () => {
    const { result } = renderHook(() => useToast())

    act(() => {
      result.current.addToast('Still here', 'error')
    })

    act(() => {
      vi.advanceTimersByTime(3999)
    })

    expect(result.current.toasts).toHaveLength(1)
  })

  it('manually removes a toast by id', () => {
    const { result } = renderHook(() => useToast())

    act(() => {
      result.current.addToast('Dismiss me', 'info')
    })

    const { id } = result.current.toasts[0]

    act(() => {
      result.current.removeToast(id)
    })

    expect(result.current.toasts).toHaveLength(0)
  })

  it('only removes the targeted toast when multiple exist', () => {
    const { result } = renderHook(() => useToast())

    act(() => {
      result.current.addToast('First', 'info')
      result.current.addToast('Second', 'success')
    })

    const firstId = result.current.toasts[0].id

    act(() => {
      result.current.removeToast(firstId)
    })

    expect(result.current.toasts).toHaveLength(1)
    expect(result.current.toasts[0].message).toBe('Second')
  })

  it('assigns a unique id to each toast', () => {
    const { result } = renderHook(() => useToast())

    act(() => {
      result.current.addToast('A', 'info')
      result.current.addToast('B', 'info')
    })

    const [a, b] = result.current.toasts
    expect(a.id).not.toBe(b.id)
  })
})
