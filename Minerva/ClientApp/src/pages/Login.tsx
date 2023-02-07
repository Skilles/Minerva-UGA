import React, {useState} from 'react';
import {Link, useNavigate} from "react-router-dom";
import {TextInput, Checkbox, Button, Group, Box, Alert, Container, Title, Paper, PasswordInput, Anchor, Text} from '@mantine/core';
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
        }
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
        <Container size={520} my={80}>
            <Title
                align="center"
                sx={(theme) => ({ fontFamily: `Greycliff CF, ${theme.fontFamily}`, fontWeight: 900 })}
            >
                Sign In
            </Title>
            <Text color="dimmed" size="sm" align="center" mt={5}>
                Do not have an account yet?{' '}
                <Link to={'/register'}>
                    Create account
                </Link>
            </Text>

            <Paper withBorder shadow="md" p={30} mt={30} radius="md">
                {error && <Alert title="Error" color="red">{error}</Alert>}
                <form onSubmit={form.onSubmit(handleLogin)}>
                    <TextInput withAsterisk
                               label="Email"
                               placeholder="your@email.com"
                               {...form.getInputProps('email')} />
                    <PasswordInput withAsterisk
                                   label="Password"
                                   placeholder="********"
                                   type="password"
                                   {...form.getInputProps('password')} />
                    <Group position="apart" mt="lg">
                        <Checkbox 
                            label="Remember me"
                            sx={{ lineHeight: 1 }} 
                            {...form.getInputProps('rememberMe', { type: 'checkbox' })} 
                        />
                        <Anchor<'a'> onClick={(event) => event.preventDefault()} href="#" size="sm">
                            Forgot password?
                        </Anchor>
                    </Group>
                    <Button type="submit" fullWidth mt="xl">
                        Sign in
                    </Button>
                </form>
            </Paper>
        </Container>
    );
};

export default Login;