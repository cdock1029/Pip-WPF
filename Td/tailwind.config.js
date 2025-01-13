/** @type {import('tailwindcss').Config} */
module.exports = {
    content: ["./Layout/**/*.{html,cshtml,razor,js}", "./Pages/**/*.{html,cshtml,razor,js}", "./Components/**/*.{html,cshtml,razor,js}", "./wwwroot/**/*.{html,cshtml,razor,js}"],
    important: '#app',
    theme: {
        extend: {},
    },
    plugins: [],
}

