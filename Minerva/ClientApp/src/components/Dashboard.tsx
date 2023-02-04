import React from 'react';

import { User } from "../hooks/useUser";
import {Container} from "@mantine/core";


export interface DashboardProps {
    user: User
}

export default function Dashboard({ user }: DashboardProps) {
    return (
        <Container mt="md">
            <h1>Hello {user.firstName}!</h1>
        </Container>
    );
}