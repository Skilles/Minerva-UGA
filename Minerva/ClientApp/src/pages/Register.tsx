import React, {useState} from 'react';
import {TextInput, Checkbox, Button, Group, Box, Alert} from '@mantine/core';
import {useForm} from "@mantine/form";


import { useAuth } from '../hooks/useAuth';
import {useNavigate} from "react-router-dom";

const Register = () => {
    const { register } = useAuth();
    const [error, setError] = useState('');
    const navigate = useNavigate();

    const form = useForm({
        initialValues: {
            firstName: '',
            lastName: '',
            email: '',
            password: '',
            rememberMe: false,
        },

        validate: {
            firstName: (value) => (value.length > 1 ? null : 'First name must be at least 2 characters long'),
            lastName: (value) => (value.length > 1 ? null : 'Last name must be at least 2 characters long'),
            email: (value) => (/^\S+@\S+$/.test(value) ? null : 'Invalid email'),
            password: (value) => (value.length > 5 ? null : 'Password must be at least 6 characters long'),
        },
    });

    const handleRegister = ({ firstName, lastName, email, password }: any) => {
        register(firstName, lastName, email, password).then(() => {
            navigate('/')
        }).catch((error) => {
            setError(error);
        });
    };

    return (
        <Box sx={{ maxWidth: 500 }} mx="auto">
            {error && <Alert title="Error" color="red">{error}</Alert>}
            <form onSubmit={form.onSubmit(handleRegister)}>
                <TextInput
                    withAsterisk
                    label="First Name"
                    placeholder="Jim"
                    {...form.getInputProps('firstName')}
                />

                <TextInput
                    withAsterisk
                    label="Last Name"
                    placeholder="Smith"
                    {...form.getInputProps('lastName')}
                />
                
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
                    <Button type="submit">Register</Button>
                </Group>
            </form>
        </Box>
    );
};

export default Register;