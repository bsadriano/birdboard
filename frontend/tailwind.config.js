/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./index.html",
    "./src/**/*.{js,ts,jsx,tsx}",
  ],
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
        default: "var(--text-default-color)",
        accent: "var(--text-accent-color)",
        "accent-light": "var(--text-accent-light-color)",
        muted: "var(--text-muted-color)",
        "muted-light": "var(--text-muted-light-color)",
        error: "var(--text-error-color)",
        page: "var(--page-background-color)",
        card: "var(--card-background-color)",
        button: "var(--button-background-color)",
        header: "var(--header-background-color)",
      },
    },
  },
  plugins: [
    function ({ addUtilities }) {
      addUtilities({
        ".button": {
          "@apply bg-button text-white no-underline rounded-lg text-sm py-2 px-5": {},
          "box-shadow": "0 2px 7px 0 #b0eaff",
          "&.is-outlined": {
            "@apply bg-card border border-accent text-accent": {},
          },
        },
        ".card": {
          "@apply bg-card p-5 rounded-lg shadow": {},
          color: "var(--text-default-color)",
        },
        a: {
          "@apply no-underline text-blue-600 hover:text-blue-800":
            {},
        },
      })
    }
  ],
}

