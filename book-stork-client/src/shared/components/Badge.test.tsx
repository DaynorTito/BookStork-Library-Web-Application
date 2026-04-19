import React from 'react'
import { render, screen } from '@testing-library/react'
import { describe, it, expect } from 'vitest'
import Badge, { getStatusVariant } from './Badge'

describe('Badge component', () => {
  it('renders its children', () => {
    render(<Badge>Available</Badge>)
    expect(screen.getByText('Available')).toBeInTheDocument()
  })

  it('renders as a <span>', () => {
    render(<Badge>Test</Badge>)
    expect(screen.getByText('Test').tagName).toBe('SPAN')
  })

  it('applies a custom className', () => {
    render(<Badge className="custom-class">Test</Badge>)
    expect(screen.getByText('Test')).toHaveClass('custom-class')
  })

  it('uses "default" variant when none is provided', () => {
    render(<Badge>Default</Badge>)
    // default variant includes bg-gray-100 classes
    expect(screen.getByText('Default')).toHaveClass('bg-gray-100')
  })

  it('applies correct classes for success variant', () => {
    render(<Badge variant="success">Success</Badge>)
    expect(screen.getByText('Success')).toHaveClass('bg-emerald-100')
  })

  it('applies correct classes for warning variant', () => {
    render(<Badge variant="warning">Warning</Badge>)
    expect(screen.getByText('Warning')).toHaveClass('bg-amber-100')
  })

  it('applies correct classes for error variant', () => {
    render(<Badge variant="error">Error</Badge>)
    expect(screen.getByText('Error')).toHaveClass('bg-red-100')
  })

  it('applies correct classes for info variant', () => {
    render(<Badge variant="info">Info</Badge>)
    expect(screen.getByText('Info')).toHaveClass('bg-primary-100')
  })
})

describe('getStatusVariant', () => {
  it.each([
    ['Available', 'success'],
    ['Loaned', 'error'],
    ['Reserved', 'warning'],
    ['Active', 'info'],
    ['Returned', 'success'],
    ['Overdue', 'error'],
    ['Fulfilled', 'success'],
    ['Cancelled', 'default'],
    ['Expired', 'default'],
    ['Unknown', 'default'],
    [null, 'default'],
    [undefined, 'default'],
  ] as const)('maps "%s" → "%s"', (status, expected) => {
    expect(getStatusVariant(status)).toBe(expected)
  })
})
