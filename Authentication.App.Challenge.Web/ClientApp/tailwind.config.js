module.exports = {
    content: [
        "./src/**/*.{js,ts,jsx,tsx}"
    ],
    theme: {
        extend: {
            colors: {
                text: '#333333',
                'text-secondary': '#828282',
                button: "#2F80ED",
                'button-hover': "#245fb3",
                primary: "#BDBDBD"
            },
        },
        container: {
            padding: {
                DEFAULT: '1rem',
                sm: '2rem',
                lg: '4rem',
                xl: '5rem',
                '2xl': '6rem',
            },
            center: true,
        },
    },
    plugins: [],
}
