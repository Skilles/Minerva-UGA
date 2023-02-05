import axios from "axios";
import {User} from "./models/user";

const authHeader = (user: User) => {
    return { Authorization: `Bearer ${user.authToken}` };
}

export const defaultQueryFn = async ({ queryKey }: any) => {
    const { data } = await axios.get(`api/${queryKey[0]}`);
    return data;
};

export const fetch = async (queryKey: string) => {
    const { data } = await axios.get(`api/${queryKey}`);
    return data;
};

export const post = async (queryKey: string, postData: any) => {
    const { data } = await axios.post(`api/${queryKey}`, postData);
    return data;
}

export const postWithAuth = async (queryKey: string, postData: any, user: User) => {
    const { data } = await axios.post(`api/${queryKey}`, postData, { headers: authHeader(user) });
    return data;
}

export const fetchWithAuth = async (queryKey: string, user: User) => {
    const { data } = await axios.get(`api/${queryKey}`, { headers: authHeader(user) });
    return data;
}