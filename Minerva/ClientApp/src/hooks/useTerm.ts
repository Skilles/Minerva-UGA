import React, {useContext, useState} from "react";
import {TermContext} from "../context/TermContext";


export default function useTerm() {
    const {term, setTerm} = useContext(TermContext);
    
    return { term, setTerm };
}