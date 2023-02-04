import {useContext} from 'react';

import {AuthContext, AuthContextInterface} from "../context/AuthContext";


export const useAuth = () => {
    const context = useContext(AuthContext) as AuthContextInterface;
    if (context === undefined) {
        throw new Error('useAuthContext must be used within a AuthProvider');
    }
    return context;
};