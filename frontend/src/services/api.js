import axios from 'axios';

import authService from './authService';

const api = axios.create({

    baseURL: 'https://localhost:5064/api',

    headers: {
        'Content-Type': 'application/json',
    },
});

// Request interceptor
api.interceptors.request.use(

    (config) => {

        const token =
            localStorage.getItem(
                'access_token'
            );

        if (token) {

            config.headers.Authorization =
                `Bearer ${token}`;
        }

        return config;
    },

    (error) => {

        return Promise.reject(error);
    }
);

// Response interceptor
api.interceptors.response.use(

    (response) => response,

    async (error) => {

        if (error.response?.status === 401) {

            authService.logout();

            window.location.href = '/login';
        }

        return Promise.reject(error);
    }
);

export default api;