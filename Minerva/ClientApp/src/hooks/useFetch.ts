import {useAuth} from "./useAuth";
import axios from "axios";

const useFetch = () => {
    const { user } = useAuth();

    const fetch = async (queryKey: string) => {
        const { data } = await axios.get(`api/${queryKey}`);
        return data;
    };

    const post = async (queryKey: string, postData: any) => {
        const { data } = await axios.post(`api/${queryKey}`, postData);
        return data;
    }

    const authHeader = () => {
        if (!user) {
            throw new Error("User is not authenticated");
        }
        return { Authorization: `Bearer ${user.authToken}` };
    }

    const postWithAuth = async (queryKey:string, postData: any) => {
        const { data } = await axios.post(`api/${queryKey}`, postData, { headers: authHeader() });
        return data;
    }

    const fetchWithAuth = async (queryKey:string) => {
        const { data } = await axios.get(`api/${queryKey}`, { headers: authHeader() });
        return data;
    }

    return { fetch, post, postWithAuth, fetchWithAuth };
}

export default useFetch;