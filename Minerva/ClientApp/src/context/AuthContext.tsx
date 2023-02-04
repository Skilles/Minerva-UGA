import React, { createContext } from 'react';
import { User } from '../hooks/useUser';

export interface AuthContextInterface {
    user: User | null;
    setUser: (user: User | null) => void;
    isLoggedIn: boolean;
    setIsLoggedIn: (isLoggedIn: boolean) => void;
    logout: () => void;
    login: (email: string, password: string, rememberMe: boolean) => Promise<void>;
    register: (firstName: string, lastName: string, email: string, password: string) => Promise<void>;
}

export const authContextDefaults: AuthContextInterface = {
    user: null,
    setUser: () => {},
    isLoggedIn: false,
    setIsLoggedIn: () => {},
    logout: () => {},
    login: () => Promise.resolve(),
    register: () => Promise.resolve()
}

export const AuthContext = createContext<AuthContextInterface>(authContextDefaults);