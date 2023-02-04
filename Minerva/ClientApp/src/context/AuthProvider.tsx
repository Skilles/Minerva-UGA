import React, {useEffect, useState} from "react";

import {AuthContext} from "./AuthContext";
import {post} from "../ApiFetch";
import {User, useUser} from "../hooks/useUser";
import {useLocalStorage} from "../hooks/useLocalStorage";


const isJwtValid = (jwt: string) => {
    const jwtData = jwt.split('.')[1];
    const decodedJwtJsonData = window.atob(jwtData);
    const decodedJwtData = JSON.parse(decodedJwtJsonData);
    const exp = new Date(decodedJwtData.exp * 1000);
    const now = new Date();
    return now < exp;
};

export function AuthProvider({ children }: any) {
    const { user, addUser, removeUser } = useUser();
    const { getItem } = useLocalStorage();

    const [isLoggedIn, setIsLoggedIn] = useState<boolean>(false);

    useEffect(() => {
        const cachedUser = getItem('user');
        if (!isLoggedIn && cachedUser) {
            const userObj = JSON.parse(cachedUser);
            if (isJwtValid(userObj.authToken)) {
                addUser(userObj, true);
                setIsLoggedIn(true);
            } else {
                logout();
            }
        }
    }, []);

    const login = (email: string, password: string, rememberMe: boolean) => {
        return post('login', {email: email, password: password})
            .then((data) => {
                console.log(data);
                const user: User = {
                    id: data.id,
                    firstName: data.firstName,
                    lastName: data.lastName,
                    email: email,
                    role: data.role,
                    authToken: data.jwtToken
                };
                addUser(user, rememberMe);
                setIsLoggedIn(true);
            }).catch((res) => {
                console.log(res);
                const { response: { data: { error } } } = res;
                throw error;
            });
    };

    const register = (firstName: string, lastName: string, email: string, password: string) => {
        return post('register', {firstName: firstName, lastName: lastName, email: email, password: password})
            .catch(({ response: { data: { error } } }) => {
                console.error(error);
                throw error;
            });
    };

    const logout = () => {
        removeUser();
        setIsLoggedIn(false);
    };
    
    // @ts-ignore
    return <AuthContext.Provider value={{ isLoggedIn, user, register, login, logout }}>{children}</AuthContext.Provider>;
}