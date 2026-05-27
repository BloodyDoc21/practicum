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
    CircularProgress
}
    from '@mui/material';

const validationSchema = Yup.object({

    username: Yup.string()
        .min(3)
        .required('Введите логин'),

    email: Yup.string()
        .email('Некорректный email')
        .required('Введите email'),

    password: Yup.string()
        .min(6)
        .required('Введите пароль'),

    confirmPassword: Yup.string()
        .oneOf(
            [Yup.ref('password')],
            'Пароли не совпадают'
        )
        .required('Подтвердите пароль')
});

const RegisterForm = () => {
    const { register } = useAuth();

    const navigate = useNavigate();

    const [error, setError]
        = useState('');
    const [loading, setLoading]
        = useState(false);

    const formik = useFormik({

        initialValues:
        {
            username: '',
            email: '',
            password: '',
            confirmPassword: ''
        },

        validationSchema,

        onSubmit: async (values) => {
            console.log("submit works");
            const result =
                await register(
                    values.username,
                    values.email,
                    values.password,
                    values.confirmPassword
                );

            if (result.success) {
                navigate('/login');
            }
            else {
                if (!result.error?.response) {
                    setError(
                        'Сервер недоступен'
                    );
                }

                else if (
                    result.error.response.status === 400
                ) {
                    setError(
                        'Некорректные данные'
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
                    Регистрация
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
                        label="Email"
                        name="email"
                        value={
                            formik.values.email
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

                    <TextField
                        fullWidth
                        margin="normal"
                        type="password"
                        label="Подтверждение"
                        name="confirmPassword"
                        value={
                            formik.values.confirmPassword
                        }
                        onChange={
                            formik.handleChange
                        }
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
                                : 'Зарегистрироваться'
                        }
                    </Button>

                    <Box mt={2}>

                        <Link to="/login">
                            Уже есть аккаунт?
                        </Link>

                    </Box>

                </form>

            </Paper>

        </Container>
    );
};

export default RegisterForm;