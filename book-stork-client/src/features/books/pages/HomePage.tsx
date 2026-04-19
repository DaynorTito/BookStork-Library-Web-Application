import React, { useEffect } from 'react'
import { Link } from 'react-router-dom'
import { Library, Bookmark, Heart } from 'lucide-react'
import { useBooks } from '@/features/books/hooks/useBooks'
import BookCard from './BookCard'
import { PageLoader } from '@/shared/components/ui/LoadingSpinner'

const FEATURES = [
  {
    Icon: Library,
    title: 'Large Catalog',
    desc: 'Thousands of titles across every genre.',
  },
  {
    Icon: Bookmark,
    title: 'Borrow & Reserve',
    desc: 'Borrow available books or reserve ones that are checked out.',
  },
  {
    Icon: Heart,
    title: 'Wishlist',
    desc: 'Save books you want to read and get notified when they are available.',
  },
]

export default function HomePage() {
  const { list, isLoading, fetch } = useBooks()

  useEffect(() => {
    fetch({ PageSize: 8 })
  // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [])

  return (
    <div className="space-y-12">
      {/* Hero */}
      <section className="py-14 text-center">
        <h1 className="text-4xl font-bold text-gray-900 dark:text-gray-100 mb-4 leading-tight">
          Your Virtual Library
        </h1>
        <p className="text-lg text-gray-500 dark:text-gray-400 mb-8 max-w-xl mx-auto">
          Discover, borrow, and reserve books from our ever-growing collection.
          Create an account to manage your reading list.
        </p>
        <div className="flex items-center justify-center gap-3 flex-wrap">
          <Link to="/catalog" className="btn-primary px-8 py-3 text-base">
            Browse Catalog
          </Link>
          <Link to="/register" className="btn-secondary px-8 py-3 text-base">
            Join for free
          </Link>
        </div>
      </section>

      {/* Feature highlights */}
      <section className="grid grid-cols-1 sm:grid-cols-3 gap-6">
        {FEATURES.map(({ Icon, title, desc }) => (
          <div
            key={title}
            className="rounded-lg border border-gray-200 dark:border-dark-border bg-white dark:bg-dark-card p-6 text-center"
          >
            <div className="flex justify-center mb-3">
              <Icon size={36} className="text-primary-700 dark:text-primary-400" />
            </div>
            <h3 className="font-semibold text-gray-900 dark:text-gray-100 mb-1">{title}</h3>
            <p className="text-sm text-gray-500 dark:text-gray-400">{desc}</p>
          </div>
        ))}
      </section>

      {/* Featured books */}
      <section>
        <div className="flex items-center justify-between mb-6">
          <h2 className="text-2xl font-semibold text-gray-900 dark:text-gray-100">
            Featured Books
          </h2>
          <Link to="/catalog" className="text-sm text-primary-700 dark:text-primary-400 hover:underline">
            View all
          </Link>
        </div>

        {isLoading ? (
          <PageLoader />
        ) : (
          <div className="grid grid-cols-2 sm:grid-cols-3 md:grid-cols-4 gap-4">
            {(list?.items ?? []).map((book) => (
              <BookCard key={book.id} book={book} />
            ))}
          </div>
        )}
      </section>
    </div>
  )
}
