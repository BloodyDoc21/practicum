import axios from 'axios';

const API_URL = 'http://localhost:5064/api';

const authService = {

    // Регистрация
    async register(username, email, password, confirmPassword) {

        const response = await axios.post(
            `${API_URL}/auth/register`,
            {
                username,
                email,
                password,
                confirmPassword
            }
        );

        return response.data;
    },

    // Вход
    async login(
        username,
        password,
        rememberMe
    ) {

        const response = await axios.post(
            `${API_URL}/auth/login`,
            {
                username,
                password
            },
            {
                withCredentials: true
            }
        );

        if (response.data.accessToken) {

            if (rememberMe) {
                localStorage.setItem(
                    'access_token',
                    response.data.accessToken
                );
            }
            else {
                sessionStorage.setItem(
                    'access_token',
                    response.data.accessToken
                );
            }
        }

        return response.data;
    },

    // Выход
    logout() {

        localStorage.removeItem('access_token');

        sessionStorage.removeItem('access_token');
    },

    // Проверка авторизации
    isAuthenticated() {

        return !!(
            localStorage.getItem('access_token')
            ||
            sessionStorage.getItem('access_token')
        );
    },

    // Получение токена
    getToken() {

        return (
            localStorage.getItem('access_token')
            ||
            sessionStorage.getItem('access_token')
        );
    }
};

export default authService;