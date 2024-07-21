/** @type {import('tailwindcss').Config} */
module.exports = {
  content: ["./src/**/*.{html,js,tsx}"],
  theme: {
    fontSize: {
      xs: "0.75rem",
      sm: "0.875rem",
      base: "1rem",
      lg: "1.125rem",
      xl: "1.25rem",
      "2xl": "1.5rem",
      "3xl": "1.875rem",
      "4xl": "2.25rem",
      "5xl": "3rem",
    },
    extend: {
      boxShadow: {
        DEFAULT: "0 0 5px 0 rgba(0, 0, 0, 0.08)",
      },
      colors: {
        'grey-light': '#F5F6F9',
        'grey': 'rgba(0, 0, 0, 0.4)'
      },
    },
  },
  plugins: [],
}

