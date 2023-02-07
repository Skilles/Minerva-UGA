import React from 'react';
import {Route, Routes} from 'react-router-dom';

import {QueryClient, QueryClientProvider} from "react-query";

import AppRoutes from './AppRoutes';
import Layout from './components/Layout';
import './custom.css';
import {AuthProvider} from "./context/AuthProvider";
import {TermProvider} from "./context/TermProvider";
import axios from "axios";


const defaultQueryFn = async ({ queryKey }: any) => {
    const { data } = await axios.get(`api/${queryKey[0]}`);
    return data;
};

// provide the default query function to your app with defaultOptions
const queryClient = new QueryClient({
    defaultOptions: {
        queries: {
            queryFn: defaultQueryFn,
        },
    },
})

export default function App() {
    return (
        <QueryClientProvider client={queryClient}>
            <AuthProvider>
                <TermProvider>
                    <Layout>
                        <Routes>
                            {AppRoutes.map((route, index) => {
                                const {element, ...rest} = route;
                                return <Route key={index} {...rest} element={element}/>;
                            })}
                        </Routes>
                    </Layout>
                </TermProvider>
            </AuthProvider>
        </QueryClientProvider>
    );
}
