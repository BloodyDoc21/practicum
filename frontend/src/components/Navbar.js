import React from 'react';

import {
    AppBar,
    Toolbar,
    Typography,
    Button,
    Box
}
    from '@mui/material';

import {
    Link,
    useNavigate
}
    from 'react-router-dom';

import { useAuth }
    from '../context/AuthContext';

const Navbar = () => {
    const {
        isAuthenticated,
        logout
    } = useAuth();

    const navigate = useNavigate();

    const handleLogout = () => {
        logout();

        navigate('/login');
    };

    return (

        <AppBar position="static">

            <Toolbar>

                <Typography
                    variant="h6"
                    sx={{ flexGrow: 1 }}
                >

                    <Link
                        to="/"
                        style={{
                            color: 'white',
                            textDecoration: 'none'
                        }}
                    >
                        CleanLife
                    </Link>

                </Typography>

                {isAuthenticated ? (

                    <Box>

                        <Button
                            color="inherit"
                            onClick={handleLogout}
                        >
                            Выйти
                        </Button>

                    </Box>

                ) : (

                    <Box>

                        <Button
                            color="inherit"
                            component={Link}
                            to="/login"
                        >
                            Вход
                        </Button>

                        <Button
                            color="inherit"
                            component={Link}
                            to="/register"
                        >
                            Регистрация
                        </Button>

                    </Box>
                )}

            </Toolbar>

        </AppBar>
    );
};

export default Navbar;