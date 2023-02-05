import React, {useEffect, useState} from 'react';
import {Flex, MultiSelect, Select, SelectItem, Stack} from "@mantine/core";
import {Course, Section, Subject} from "../models/planner";
import {User} from "../models/user";
import {fetchWithAuth, postWithAuth} from "../ApiFetch";
import {useUser} from "../hooks/useUser";
import useTerm from "../hooks/useTerm";
import {Term} from "../models/term";
import {useAuth} from "../hooks/useAuth";

export interface PlannerControlProps {
    initialCourses: Course[];
    term: Term;
}

const fetchSubjects = async (termId: number, user: User): Promise<Subject[]> => {
    return fetchWithAuth(`search/subjects?id=${termId}`, user);
}

const fetchCourses = async (subjectId: string, user: User): Promise<Course[]> => {
    return fetchWithAuth(`search/courses?id=${subjectId}`, user);
}

const fetchSections = async (crns: number[], user: User): Promise<Section[]> => {
    const data = {
        crns: crns
    }
    return postWithAuth(`search/bulkSections`, data, user);
}

interface SubjectData {
    label: string;
    key: string;
    value: string;
}


// This component contains a select element to select a subject. Once a subject is selected, the component will render a list of courses for that subject. The user can select as many courses as they like and add them to the planner sidebar.
export default function PlannerControl({ initialCourses, term }: PlannerControlProps) {
    const [populatedCourses, setPopulatedCourses] = useState<Course[]>(initialCourses);
    const [selectableCourses, setSelectableCourses] = useState<SelectItem[]>([]);
    const [subjects, setSubjects] = useState<SelectItem[]>([]);
    
    const [currentSections, setCurrentSections] = useState<Section[]>([]);
    
    const [selectedSubject, setSelectedSubject] = useState<string | null>(null);
    const [selectedCourses, setSelectedCourses] = useState<string[]>([]);
    
    const [courseDict, setCourseDict] = useState<Record<string, Course>>({});
    
    const { isLoggedIn, user } = useAuth();
    
    useEffect(() => {
        if (term && isLoggedIn) {
            getSubjects();
        }
    }, [term, isLoggedIn]);
    
    const setSubject = (subjectId: string | null) => {
        if (subjectId) {
            setSelectedSubject(subjectId);
            fetchCourses(subjectId, user!).then((courses) => {
                setPopulatedCourses(courses);
                
                setSelectableCourses(courses.map((course): SelectItem => {
                    courseDict[course.name] = course;
                    return {
                        label: course.name,
                        key: course.name,
                        value: course.name
                    };
                }));
            });
        }
    }
    
    const setCourses = (courses: string[]) => {
        if (courses) {
            setSelectedCourses(courses);
            fetchSections(courseDict[courses[0]].sections, user!).then((sections) => {
                setCurrentSections(sections);
            });
        }
    }
    
    const getSubjects = () => {
        console.log("Getting subjects for term: " + term!.termId);
        if (subjects.length > 0) {
            return subjects;
        }
        fetchSubjects(term!.termId, user!).then((subjects) => {
            setSubjects(subjects.map((subject): SubjectData => {
                    return {
                        label: `${subject.name} (${subject.subjectId})`,
                        key: subject.subjectId,
                        value: subject.subjectId
                    };
                })
            );
        });
    }
    
    return (
        <Flex w={450}>
            <Stack mx={20} w="100%">
                <Select
                    label="Select a major"
                    placeholder="Pick one"
                    searchable
                    clearable
                    nothingFound="No majors found"
                    data={subjects}
                    value={selectedSubject}
                    onChange={setSubject}
                />
                <Stack mx={10}>
                    {selectedSubject && 
                        <MultiSelect
                            label="Select courses"
                            placeholder="Pick multiple"
                            searchable
                            nothingFound="No courses found" 
                            data={selectableCourses}
                            value={selectedCourses}
                            onChange={setCourses}
                        >
                            {/*{currentSections.map((section) => {
                                return (
                                    <div>
                                        <h3>{section.professor}</h3>
                                        <p>{section.crn}</p>
                                    </div>
                                );
                            })}*/}
                    </MultiSelect>}
                </Stack>
            </Stack>
        </Flex>
    );
}