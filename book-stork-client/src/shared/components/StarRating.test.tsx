import React from 'react'
import { render, screen } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { describe, it, expect, vi } from 'vitest'
import StarRating from './StarRating'

describe('StarRating component', () => {
  it('renders 5 star buttons by default', () => {
    render(<StarRating rating={3} />)
    expect(screen.getAllByRole('button')).toHaveLength(5)
  })

  it('renders the numeric rating value when showValue is true', () => {
    render(<StarRating rating={3.5} showValue />)
    expect(screen.getByText('3.5')).toBeInTheDocument()
  })

  it('hides the numeric value when showValue is false', () => {
    render(<StarRating rating={3.5} showValue={false} />)
    expect(screen.queryByText('3.5')).not.toBeInTheDocument()
  })

  it('respects a custom maxStars value', () => {
    render(<StarRating rating={2} maxStars={10} showValue={false} />)
    expect(screen.getAllByRole('button')).toHaveLength(10)
  })

  it('star buttons are disabled when not interactive', () => {
    render(<StarRating rating={3} interactive={false} />)
    screen.getAllByRole('button').forEach((btn) => {
      expect(btn).toBeDisabled()
    })
  })

  it('star buttons are enabled when interactive', () => {
    render(<StarRating rating={3} interactive onRate={vi.fn()} />)
    screen.getAllByRole('button').forEach((btn) => {
      expect(btn).toBeEnabled()
    })
  })

  it('calls onRate with the correct value when a star is clicked', async () => {
    const onRate = vi.fn()
    render(<StarRating rating={0} interactive onRate={onRate} />)

    const buttons = screen.getAllByRole('button')
    await userEvent.click(buttons[2]) // 3rd star → rating 3

    expect(onRate).toHaveBeenCalledWith(3)
  })

  it('does not call onRate when not interactive', async () => {
    const onRate = vi.fn()
    render(<StarRating rating={3} interactive={false} onRate={onRate} />)

    const buttons = screen.getAllByRole('button')
    await userEvent.click(buttons[0])

    expect(onRate).not.toHaveBeenCalled()
  })

  it('renders correct aria-labels for each star', () => {
    render(<StarRating rating={1} showValue={false} />)
    expect(screen.getByLabelText('1 star')).toBeInTheDocument()
    expect(screen.getByLabelText('2 stars')).toBeInTheDocument()
    expect(screen.getByLabelText('5 stars')).toBeInTheDocument()
  })
})
