import React from 'react'
import { render, screen } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { describe, it, expect, vi } from 'vitest'
import Pagination from './Pagination'

describe('Pagination component', () => {
  it('renders nothing when totalPages is 1', () => {
    const { container } = render(
      <Pagination page={1} totalPages={1} onPageChange={vi.fn()} />,
    )
    expect(container.firstChild).toBeNull()
  })

  it('renders navigation when totalPages > 1', () => {
    render(<Pagination page={1} totalPages={5} onPageChange={vi.fn()} />)
    expect(screen.getByRole('navigation')).toBeInTheDocument()
  })

  it('disables the Prev button on the first page', () => {
    render(<Pagination page={1} totalPages={5} onPageChange={vi.fn()} />)
    expect(screen.getByLabelText('Previous page')).toBeDisabled()
  })

  it('disables the Next button on the last page', () => {
    render(<Pagination page={5} totalPages={5} onPageChange={vi.fn()} />)
    expect(screen.getByLabelText('Next page')).toBeDisabled()
  })

  it('enables both Prev and Next on a middle page', () => {
    render(<Pagination page={3} totalPages={5} onPageChange={vi.fn()} />)
    expect(screen.getByLabelText('Previous page')).toBeEnabled()
    expect(screen.getByLabelText('Next page')).toBeEnabled()
  })

  it('calls onPageChange with page - 1 when Prev is clicked', async () => {
    const onPageChange = vi.fn()
    render(<Pagination page={3} totalPages={5} onPageChange={onPageChange} />)

    await userEvent.click(screen.getByLabelText('Previous page'))

    expect(onPageChange).toHaveBeenCalledWith(2)
  })

  it('calls onPageChange with page + 1 when Next is clicked', async () => {
    const onPageChange = vi.fn()
    render(<Pagination page={3} totalPages={5} onPageChange={onPageChange} />)

    await userEvent.click(screen.getByLabelText('Next page'))

    expect(onPageChange).toHaveBeenCalledWith(4)
  })

  it('calls onPageChange with the correct page when a numbered button is clicked', async () => {
    const onPageChange = vi.fn()
    render(<Pagination page={1} totalPages={3} onPageChange={onPageChange} />)

    await userEvent.click(screen.getByText('3'))

    expect(onPageChange).toHaveBeenCalledWith(3)
  })

  it('marks the current page button with aria-current="page"', () => {
    render(<Pagination page={2} totalPages={3} onPageChange={vi.fn()} />)
    expect(screen.getByText('2')).toHaveAttribute('aria-current', 'page')
  })

  it('renders an ellipsis when pages are non-contiguous', () => {
    // page 1, totalPages 10 — there is a gap between page 2 and page 10
    render(<Pagination page={1} totalPages={10} onPageChange={vi.fn()} />)
    expect(screen.getByText('…')).toBeInTheDocument()
  })
})
