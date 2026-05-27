import React from 'react';

import {
    BrowserRouter,
    Routes,
    Route
} from 'react-router-dom';

import { AuthProvider } from './context/AuthContext';

import PrivateRoute from './components/PrivateRoute';
import PublicRoute from './components/PublicRoute';

import LoginForm from './components/LoginForm';
import RegisterForm from './components/RegisterForm';

import Navbar from './components/Navbar';

function Home() {
    return (
        <div style={{ padding: '20px' }}>
            <h1>CleanLife</h1>

            <p>
                Система отказа
                от вредных привычек
            </p>
        </div>
    );
}

function App() {
    return (

        <BrowserRouter>

            <AuthProvider>

                <Navbar />

                <Routes>

                    <Route
                        path="/login"
                        element={
                            <PublicRoute>
                                <LoginForm />
                            </PublicRoute>
                        }
                    />

                    <Route
                        path="/register"
                        element={
                            <PublicRoute>
                                <RegisterForm />
                            </PublicRoute>
                        }
                    />

                    <Route
                        path="/"
                        element={
                            <PrivateRoute>
                                <Home />
                            </PrivateRoute>
                        }
                    />

                </Routes>

            </AuthProvider>

        </BrowserRouter>
    );
}

export default App;