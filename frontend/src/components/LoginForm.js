import React,
{
    useState
}
    from 'react';

import { useFormik } from 'formik';

import * as Yup from 'yup';

import { useAuth }
    from '../context/AuthContext';

import {
    useNavigate,
    Link
}
    from 'react-router-dom';

import {
    TextField,
    Button,
    Container,
    Typography,
    Box,
    Alert,
    Paper,
    CircularProgress,
    Checkbox,
    FormControlLabel

}
    from '@mui/material';

const validationSchema = Yup.object({

    username: Yup.string()
        .required('Введите логин'),

    password: Yup.string()
        .required('Введите пароль')
});

const LoginForm = () => {
    const { login } = useAuth();

    const navigate = useNavigate();

    const [error, setError]
        = useState('');

    const [loading, setLoading]
        = useState(false);

    const formik = useFormik({

        initialValues:
        {
            username: '',
            password: '',
            rememberMe: false
        },

        validationSchema,

        onSubmit: async (values) => {
            setLoading(true);
            const result =
                await login(
                    values.username,
                    values.password,
                    values.rememberMe
                );
            setLoading(false);

            if (result.success) {
                navigate('/');
            }
            else {
                if (!result.error?.response) {
                    setError(
                        'Сервер недоступен'
                    );
                }

                else if (
                    result.error.response.status === 401
                ) {
                    setError(
                        'Неверный логин или пароль'
                    );
                }

                else if (
                    result.error.response.status === 500
                ) {
                    setError(
                        'Ошибка сервера'
                    );
                }

                else {
                    setError(
                        'Неизвестная ошибка'
                    );
                }
            }
        }
    });

    return (
        <Container maxWidth="sm">

            <Paper
                elevation={3}
                sx={{ p: 4, mt: 8 }}
            >

                <Typography
                    variant="h4"
                    align="center"
                >
                    Вход
                </Typography>

                {error && (

                    <Alert severity="error">
                        {error}
                    </Alert>
                )}

                <form
                    onSubmit={
                        formik.handleSubmit
                    }
                >

                    <TextField
                        fullWidth
                        margin="normal"
                        label="Логин"
                        name="username"
                        value={
                            formik.values.username
                        }
                        onChange={
                            formik.handleChange
                        }
                    />

                    <TextField
                        fullWidth
                        margin="normal"
                        type="password"
                        label="Пароль"
                        name="password"
                        value={
                            formik.values.password
                        }
                        onChange={
                            formik.handleChange
                        }
                    />
                    <FormControlLabel
                        control={
                            <Checkbox
                                name="rememberMe"
                                checked={
                                    formik.values.rememberMe
                                }
                                onChange={
                                    formik.handleChange
                                }
                            />
                        }
                        label="Запомнить меня"
                    />

                    <Button
                        type="submit"
                        fullWidth
                        variant="contained"
                        sx={{ mt: 2 }}
                        disabled={loading}
                    >
                        {
                            loading
                                ? <CircularProgress size={24} />
                                : 'Войти'
                        }
                    </Button>

                    <Box mt={2}>

                        <Link to="/register">
                            Нет аккаунта?
                        </Link>

                    </Box>

                </form>

            </Paper>

        </Container>
    );
};

export default LoginForm;