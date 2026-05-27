import React,
{
    createContext,
    useState,
    useContext,
    useEffect
}
    from 'react';

import authService from '../services/authService';

const AuthContext = createContext();

export const useAuth = () =>
    useContext(AuthContext);

export const AuthProvider = ({ children }) => {
    const [isAuthenticated,
        setIsAuthenticated]
        = useState(false);

    useEffect(() => {
        setIsAuthenticated(
            authService.isAuthenticated()
        );
    }, []);

    // Регистрация
    const register = async (
        username,
        email,
        password,
        confirmPassword
    ) => {
        try {
            await authService.register(
                username,
                email,
                password,
                confirmPassword
            );

            return {
                success: true
            };
        }
        catch (error) {
            return {
                success: false,
                error: error
                    
            };
        }
    };

    // Вход
    const login = async (
        username,
        password
    ) => {
        try {
            await authService.login(
                username,
                password
            );

            setIsAuthenticated(true);

            return {
                success: true
            };
        }
        catch (error) {
            return {
                success: false,
                error: error
            };
        }
    };

    // Выход
    const logout = () => {
        authService.logout();

        setIsAuthenticated(false);
    };

    const value =
    {
        isAuthenticated,
        register,
        login,
        logout
    };

    return (
        <AuthContext.Provider value={value}>
            {children}
        </AuthContext.Provider>
    );
};