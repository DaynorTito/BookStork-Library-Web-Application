import React from 'react'
import { render, screen } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { describe, it, expect, vi } from 'vitest'
import Modal from './Modal'

describe('Modal component', () => {
  it('renders nothing when isOpen is false', () => {
    render(<Modal isOpen={false} onClose={vi.fn()}>Content</Modal>)
    expect(screen.queryByRole('dialog')).not.toBeInTheDocument()
  })

  it('renders the dialog when isOpen is true', () => {
    render(<Modal isOpen onClose={vi.fn()}>Content</Modal>)
    expect(screen.getByRole('dialog')).toBeInTheDocument()
  })

  it('renders children inside the dialog', () => {
    render(<Modal isOpen onClose={vi.fn()}>Hello World</Modal>)
    expect(screen.getByText('Hello World')).toBeInTheDocument()
  })

  it('renders the title when provided', () => {
    render(<Modal isOpen onClose={vi.fn()} title="My Title">Body</Modal>)
    expect(screen.getByText('My Title')).toBeInTheDocument()
  })

  it('calls onClose when the close button is clicked', async () => {
    const onClose = vi.fn()
    render(<Modal isOpen onClose={onClose}>Body</Modal>)

    await userEvent.click(screen.getByLabelText('Close modal'))

    expect(onClose).toHaveBeenCalledOnce()
  })

  it('calls onClose when the backdrop is clicked', async () => {
    const onClose = vi.fn()
    render(<Modal isOpen onClose={onClose}>Body</Modal>)

    // The backdrop is the aria-hidden overlay div
    const backdrop = document.querySelector('[aria-hidden="true"]') as HTMLElement
    await userEvent.click(backdrop)

    expect(onClose).toHaveBeenCalledOnce()
  })

  it('calls onClose when Escape key is pressed', async () => {
    const onClose = vi.fn()
    render(<Modal isOpen onClose={onClose}>Body</Modal>)

    await userEvent.keyboard('{Escape}')

    expect(onClose).toHaveBeenCalledOnce()
  })

  it('does not call onClose when another key is pressed', async () => {
    const onClose = vi.fn()
    render(<Modal isOpen onClose={onClose}>Body</Modal>)

    await userEvent.keyboard('{Enter}')

    expect(onClose).not.toHaveBeenCalled()
  })

  it('sets aria-modal and aria-labelledby correctly', () => {
    render(<Modal isOpen onClose={vi.fn()} title="Accessible">Body</Modal>)
    const dialog = screen.getByRole('dialog')
    expect(dialog).toHaveAttribute('aria-modal', 'true')
    expect(dialog).toHaveAttribute('aria-labelledby', 'modal-title')
  })

  it('does not set aria-labelledby when no title is given', () => {
    render(<Modal isOpen onClose={vi.fn()}>Body</Modal>)
    const dialog = screen.getByRole('dialog')
    expect(dialog).not.toHaveAttribute('aria-labelledby')
  })
})
