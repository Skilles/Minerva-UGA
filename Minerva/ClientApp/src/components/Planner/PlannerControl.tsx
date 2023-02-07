import React, {useEffect, useState} from 'react';
import {Flex, MultiSelect, Select, SelectItem, Stack} from "@mantine/core";
import {Course, Subject} from "../../models/planner";
import {Term} from "../../models/term";
import useFetch from "../../hooks/useFetch";


export interface PlannerControlProps {
    initialCourses: Course[];
    term: Term;
    onSetCourses: (courses: CourseData[]) => void;
}

export interface CourseData {
    name: string;
    crns: number[];
    subjectId: string;
}

function PlannerCourseSelector({ courses, onChange}: { courses: Course[], onChange: (courses: string[]) => void } ) {
    const courseChoices = courses.map((course): SelectItem => {
        return {
            label: course.name,
            key: course.name,
            value: course.name
        };
    });

    return (
        <MultiSelect
            label="Select courses"
            placeholder="Pick multiple"
            searchable
            nothingFound="No courses found"
            data={courseChoices}
            onChange={onChange}
        />
    )
}

export default function PlannerControl({term, onSetCourses }: PlannerControlProps) {
    const { fetchWithAuth } = useFetch();
    
    const [subjects, setSubjects] = useState<SelectItem[]>([]);
    
    const [selectedSubjectId, setSelectedSubjectId] = useState<string | null>(null);
    const [subjectCourses, setSubjectCourses] = useState<Course[]>([]);
    
    const [courseDataLookup, setCourseDataLookup] = useState<Record<string, CourseData>>({});

    const fetchSubjects = async (termId: number): Promise<Subject[]> => {
        return fetchWithAuth(`search/subjects?id=${termId}`);
    }

    const fetchCourses = async (subjectId: string): Promise<Course[]> => {
        return fetchWithAuth(`search/courses?id=${subjectId}`);
    }
    
    useEffect(() => {
        if (term && subjects.length === 0) {
            if (subjects.length > 0) {
                return;
            }
            fetchSubjects(term!.termId).then((subjects) => {
                setSubjects(subjects.map((subject): SelectItem => {
                        return {
                            label: `${subject.name} (${subject.subjectId})`,
                            key: subject.subjectId,
                            value: subject.subjectId
                        };
                    })
                );
            });
        }
    }, [term]);
    
    const setSubject = (subjectId: string | null) => {
        if (subjectId) {
            setSelectedSubjectId(subjectId);
            fetchCourses(subjectId).then((courses) => {
                courses.forEach((course) => {
                    courseDataLookup[course.name] = {
                        name: course.name,
                        crns: course.sections,
                        subjectId: subjectId
                    }
                });
                setSubjectCourses(courses);
            });
            setCourseDataLookup(courseDataLookup);
        }
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
                    value={selectedSubjectId}
                    onChange={setSubject}
                />
                {selectedSubjectId && 
                    <PlannerCourseSelector
                        courses={subjectCourses}
                        onChange={(courses) => onSetCourses(courses.map(course => courseDataLookup[course]))}
                    /> 
                }
            </Stack>
        </Flex>
    );
}