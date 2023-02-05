import {useContext, useState} from 'react';
import { useLocalStorage } from './useLocalStorage';
import {User} from "../models/user";

export const useUser = () => {
    const [user, setUser] = useState<User | null>(null);
    const { setItem, setItemSession, removeItem } = useLocalStorage();

    const addUser = (user: User, remember: boolean) => {
        setUser(user);
        if (remember) {
            setItem('user', JSON.stringify(user));
        } else {
            setItemSession('user', JSON.stringify(user));
        }
    };

    const removeUser = () => {
        setUser(null);
        removeItem('user');
    };

    return { user, addUser, removeUser };
};