import React, {useState} from 'react';
import {useNavigate} from "react-router-dom";
import {TextInput, Checkbox, Button, Group, Box, Alert} from '@mantine/core';
import {useForm} from "@mantine/form";


import { useAuth } from '../hooks/useAuth';

const Login = () => {
    const { login } = useAuth();
    const [error, setError] = useState('');
    const navigate = useNavigate();

    const form = useForm({
        initialValues: {
            email: '',
            password: '',
            rememberMe: false,
        },

        validate: {
            email: (value) => (/^\S+@\S+$/.test(value) ? null : 'Invalid email'),
            password: (value) => (value.length > 5 ? null : 'Password must be at least 6 characters long'),
        },
    });

    const handleLogin = ({ email, password, rememberMe }: any) => {
        login(email, password, rememberMe).then(() => {
            navigate('/')
        }).catch((error) => {
            console.log(error);
            setError(error);
        });
    };

    return (
        <Box sx={{ maxWidth: 500 }} mx="auto">
            {error && <Alert title="Error" color="red">{error}</Alert>}
            <form onSubmit={form.onSubmit(handleLogin)}>
                <TextInput
                    withAsterisk
                    label="Email"
                    placeholder="your@email.com"
                    {...form.getInputProps('email')}
                />

                <TextInput
                    withAsterisk
                    label="Password"
                    placeholder="********"
                    type="password"
                    {...form.getInputProps('password')}
                />

                <Checkbox
                    mt="md"
                    label="Remember me"
                    {...form.getInputProps('rememberMe', { type: 'checkbox' })}
                />

                <Group position="right" mt="md">
                    <Button type="submit">Login</Button>
                </Group>
            </form>
        </Box>
    );
};

export default Login;