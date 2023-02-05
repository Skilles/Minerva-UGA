import React from 'react';

import {Container} from "@mantine/core";
import {User} from "../models/user";
import TermSelector from "./TermSelector";


export interface DashboardProps {
    user: User
}

export default function Dashboard({ user }: DashboardProps) {
    return (
        <Container mt="md">
            <h1>Hello {user.firstName}!</h1>
            <TermSelector />
        </Container>
    );
}