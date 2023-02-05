import React, {ReactNode, useEffect, useState} from "react";
import { TermContext } from "./TermContext";
import {Term} from "../models/term";
import {useLocalStorage} from "../hooks/useLocalStorage";


export function TermProvider ({ children }: { children: ReactNode }) {
    const [term, setTermValue] = useState<Term | null>(null);
    
    const { setItem, getItem } = useLocalStorage();
    
    useEffect(() => {
        const cachedTerm = getItem('term');
        if (cachedTerm) {
            const termObj = JSON.parse(cachedTerm);
            setTermValue(termObj);
        }
    }, []);
    
    const setTerm = (term: Term | null) => {
        setTermValue(term);
        if (term) {
            setItem('term', JSON.stringify(term));
        }
    }

    return (
        <TermContext.Provider value={{ term, setTerm }}>
            {children}
        </TermContext.Provider>
    );
}