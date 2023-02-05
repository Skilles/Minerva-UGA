
import { createContext } from 'react';
import {Term} from "../models/term";

export interface TermContextInterface {
    term: Term | null;
    setTerm: (term: Term | null) => void;
}

export const termContextDefaults: TermContextInterface = {
    term: null,
    setTerm: () => {},
}

export const TermContext = createContext<TermContextInterface>(termContextDefaults);