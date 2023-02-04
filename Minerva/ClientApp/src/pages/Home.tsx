import React from 'react';

import {useAuth} from "../hooks/useAuth";
import IntroSection from "../components/IntroSection";
import Dashboard from "../components/Dashboard";

export default function Home() {
    const { isLoggedIn, user } = useAuth();
    
    return isLoggedIn ? <Dashboard user={user!} /> : <IntroSection />;
}
