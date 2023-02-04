import { useState } from 'react';

export const useLocalStorage = () => {
    const [value, setValue] = useState<string | null>(null);

    const setItem = (key: string, value: string) => {
        localStorage.setItem(key, value);
        setValue(value);
    };
    
    const setItemSession = (key: string, value: string) => {
        sessionStorage.setItem(key, value);
        setValue(value);
    }

    const getItem = (key: string) => {
        const value = localStorage.getItem(key) ?? sessionStorage.getItem(key);
        setValue(value);
        return value;
    };

    const removeItem = (key: string) => {
        localStorage.removeItem(key);
        sessionStorage.removeItem(key);
        setValue(null);
    };

    return { value, setItem, setItemSession, getItem, removeItem };
};