import React from 'react';
import {Route, Routes} from 'react-router-dom';

import {QueryClient, QueryClientProvider} from "react-query";

import AppRoutes from './AppRoutes';
import Layout from './components/Layout';
import './custom.css';
import {defaultQueryFn} from "./ApiFetch";
import {AuthProvider} from "./context/AuthProvider";


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
        <AuthProvider>
            <QueryClientProvider client={queryClient}>
                <Layout>
                    <Routes>
                        {AppRoutes.map((route, index) => {
                            const {element, ...rest} = route;
                            return <Route key={index} {...rest} element={element}/>;
                        })}
                    </Routes>
                </Layout>
            </QueryClientProvider>
        </AuthProvider>
    );
}
