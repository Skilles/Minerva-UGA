import {useEffect, useState} from "react";

import {Term} from "../models/term";
import { Select } from "@mantine/core";
import useTerm from "../hooks/useTerm";
import useFetch from "../hooks/useFetch";

interface TermData {
    label: string;
    key: number;
    value: string;
}

const testData: TermData[] = [
    {
        label: "Fall 2021",
        key: 1,
        value: "Fall 2021"
    },
    {
        label: "Spring 2022",
        key: 2,
        value: "Spring 2022"
    }
]

export default function TermSelector() {
    const { term, setTerm } = useTerm();
    const { postWithAuth, fetchWithAuth } = useFetch();
    
    const [terms, setTerms] = useState<Record<string, TermData>>({});
    
    const [selectedTerm, setSelectedTerm] = useState<string | null>(term?.semester ?? null);
    
    
    const setTermFromSelect = (value: string | null) => {
        if (value && terms[value]) {
            console.log("Setting term to " + value);
            let term: Term = {
                termId: terms[value].key,
                semester: value
            };
            setSelectedTerm(value);
            setTerm(term);
            upsertPlanner(terms[value].key)
        }
    }
    
    const upsertPlanner = (termId: number) => {
        let data = {
            termId: termId
        }
        postWithAuth(`planner`, data)
            .then((plannerId) => {
                console.log("planner created: " + plannerId);
            }
        ).catch((error) => {
            console.log("error creating planner: " + error);
        });
    }
    
    useEffect(() => {
        fetchWithAuth('search/terms')
            .then((terms: Term[]) => {
                console.log("fetched terms: " + JSON.stringify(terms));
                let termDict: Record<string, TermData> = {};
                for (const term of terms) {
                    termDict[term.semester] = {
                        label: term.semester,
                        key: term.termId,
                        value: term.semester
                    };
                }
                console.log("termDict: " + JSON.stringify(termDict));
                setTerms(termDict);
                setSelectedTerm(terms[0].semester);
        });
    }, []);
    
    return (
        <Select 
            label="Select a term"
            placeholder="Pick one"
            onChange={setTermFromSelect}
            value={selectedTerm}
            data={Object.values(terms)}
        />
    )
        
}