import { renderHook, act } from '@testing-library/react'
import { describe, it, expect, vi, beforeEach } from 'vitest'
import { useTheme } from './useTheme'

// jsdom does not implement matchMedia — provide a minimal stub
function mockMatchMedia(prefersDark: boolean) {
  Object.defineProperty(window, 'matchMedia', {
    writable: true,
    value: vi.fn().mockImplementation((query: string) => ({
      matches: query === '(prefers-color-scheme: dark)' ? prefersDark : false,
      media: query,
      onchange: null,
      addListener: vi.fn(),
      removeListener: vi.fn(),
      addEventListener: vi.fn(),
      removeEventListener: vi.fn(),
      dispatchEvent: vi.fn(),
    })),
  })
}

describe('useTheme', () => {
  beforeEach(() => {
    localStorage.clear()
    document.documentElement.className = ''
    mockMatchMedia(false)
  })

  it('defaults to light when system preference is light and nothing is stored', () => {
    const { result } = renderHook(() => useTheme())
    expect(result.current[0]).toBe('light')
  })

  it('defaults to dark when system preference is dark and nothing is stored', () => {
    mockMatchMedia(true)
    const { result } = renderHook(() => useTheme())
    expect(result.current[0]).toBe('dark')
  })

  it('reads stored theme from localStorage instead of system preference', () => {
    localStorage.setItem('bookstork_theme', 'dark')
    const { result } = renderHook(() => useTheme())
    expect(result.current[0]).toBe('dark')
  })

  it('toggle switches from light to dark', () => {
    const { result } = renderHook(() => useTheme())
    expect(result.current[0]).toBe('light')

    act(() => {
      result.current[1]() // toggle
    })

    expect(result.current[0]).toBe('dark')
  })

  it('toggle switches from dark back to light', () => {
    localStorage.setItem('bookstork_theme', 'dark')
    const { result } = renderHook(() => useTheme())

    act(() => {
      result.current[1]()
    })

    expect(result.current[0]).toBe('light')
  })

  it('applies dark class to document root when theme is dark', () => {
    localStorage.setItem('bookstork_theme', 'dark')
    renderHook(() => useTheme())
    expect(document.documentElement.classList.contains('dark')).toBe(true)
  })

  it('removes dark class from document root when theme is light', () => {
    document.documentElement.classList.add('dark')
    localStorage.setItem('bookstork_theme', 'light')
    renderHook(() => useTheme())
    expect(document.documentElement.classList.contains('dark')).toBe(false)
  })

  it('persists the toggled theme to localStorage', () => {
    const { result } = renderHook(() => useTheme())

    act(() => {
      result.current[1]()
    })

    expect(localStorage.getItem('bookstork_theme')).toBe('dark')
  })
})
