/** @type {import('tailwindcss').Config} */
export default {
  content: ['./index.html', './src/**/*.{js,ts,jsx,tsx}'],
  darkMode: 'class',
  theme: {
    extend: {
      fontFamily: {
        display: ['Playfair Display', 'Georgia', 'serif'],
        body: ['Lato', 'sans-serif'],
      },
      colors: {
        primary: {
          50:  '#fdf2f4',
          100: '#fce7eb',
          200: '#f9d0da',
          300: '#f4a8bb',
          400: '#ec7596',
          500: '#e04a76',
          600: '#c7295a',
          700: '#a71d48',
          800: '#8b1b41',
          900: '#761a3d',
          950: '#420a1e',
        },
        accent: {
          400: '#fbbf24',
          500: '#f59e0b',
        },
        dark: {
          bg:      '#0f0e17',
          surface: '#1a1826',
          card:    '#22203a',
          border:  '#2e2b4a',
        },
      },
      boxShadow: {
        'book': '4px 4px 0px 0px rgba(139,27,65,0.15)',
        'book-hover': '6px 6px 0px 0px rgba(139,27,65,0.25)',
      },
      animation: {
        'fade-in': 'fadeIn 0.3s ease-in-out',
        'slide-up': 'slideUp 0.4s ease-out',
        'spin-slow': 'spin 3s linear infinite',
      },
      keyframes: {
        fadeIn: {
          '0%': { opacity: '0' },
          '100%': { opacity: '1' },
        },
        slideUp: {
          '0%': { opacity: '0', transform: 'translateY(16px)' },
          '100%': { opacity: '1', transform: 'translateY(0)' },
        },
      },
    },
  },
  plugins: [],
}
