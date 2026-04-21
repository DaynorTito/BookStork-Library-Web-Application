import React from 'react'
import { render, screen } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { describe, it, expect, vi } from 'vitest'
import ToastContainer from './ToastContainer'
import type { Toast } from '@/shared/hooks/useToast'

const makeToast = (overrides: Partial<Toast> = {}): Toast => ({
  id: 'toast-1',
  message: 'Test message',
  type: 'info',
  ...overrides,
})

describe('ToastContainer component', () => {
  it('renders nothing when the toasts array is empty', () => {
    const { container } = render(<ToastContainer toasts={[]} onRemove={vi.fn()} />)
    // The wrapper div is always present but has no toast children
    expect(container.querySelector('[class*="pointer-events-auto"]')).toBeNull()
  })

  it('renders the toast message', () => {
    render(<ToastContainer toasts={[makeToast({ message: 'Hello toast' })]} onRemove={vi.fn()} />)
    expect(screen.getByText('Hello toast')).toBeInTheDocument()
  })

  it('renders multiple toasts', () => {
    const toasts: Toast[] = [
      makeToast({ id: '1', message: 'First' }),
      makeToast({ id: '2', message: 'Second' }),
    ]
    render(<ToastContainer toasts={toasts} onRemove={vi.fn()} />)
    expect(screen.getByText('First')).toBeInTheDocument()
    expect(screen.getByText('Second')).toBeInTheDocument()
  })

  it('calls onRemove with the correct id when dismiss is clicked', async () => {
    const onRemove = vi.fn()
    render(
      <ToastContainer
        toasts={[makeToast({ id: 'abc', message: 'Dismiss me' })]}
        onRemove={onRemove}
      />,
    )

    await userEvent.click(screen.getByLabelText('Dismiss'))

    expect(onRemove).toHaveBeenCalledWith('abc')
  })

  it('renders the correct dismiss button for each toast', async () => {
    const onRemove = vi.fn()
    const toasts: Toast[] = [
      makeToast({ id: 'first', message: 'First' }),
      makeToast({ id: 'second', message: 'Second' }),
    ]
    render(<ToastContainer toasts={toasts} onRemove={onRemove} />)

    const dismissButtons = screen.getAllByLabelText('Dismiss')
    expect(dismissButtons).toHaveLength(2)

    await userEvent.click(dismissButtons[0])
    expect(onRemove).toHaveBeenCalledWith('first')
  })

  it.each([
    ['success', 'text-emerald-400'],
    ['error', 'text-red-400'],
    ['info', 'text-blue-400'],
    ['warning', 'text-amber-400'],
  ] as const)('applies the correct icon color for %s type', (type, colorClass) => {
    render(<ToastContainer toasts={[makeToast({ type })]} onRemove={vi.fn()} />)
    // The icon SVG element receives the color class
    const icon = document.querySelector(`.${colorClass}`)
    expect(icon).not.toBeNull()
  })
})
