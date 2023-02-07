import {CourseData} from "./PlannerControl";
import {ScheduleTimeslot, Section} from "../../models/planner";
import {Accordion, Stack} from "@mantine/core";
import {useState} from "react";

export interface PlannerCourseListProps {
    courses: CourseData[];
    addTimeslot: (timeslot: ScheduleTimeslot) => void;
    onExpand: (crns: number[]) => void;
}

interface PlannerCourseCardProps {
    course: CourseData;
    addTimeslot: (timeslot: ScheduleTimeslot) => void;
    onExpand: (crns: number[]) => void;
}

function PlannerCourseCard({ course, onExpand }: PlannerCourseCardProps) {
    const [collapsed, setCollapsed] = useState(true);
    const [sections, setSections] = useState<Section[]>([]);

    return (
            <Accordion.Item value={`${course.subjectId}  ${course.name}`} key={`${course.subjectId}  ${course.name}`}>
                <Accordion.Control>{course.subjectId} - {course.name}</Accordion.Control>
                <Accordion.Panel>
                    <Stack>
                        {sections.map(section => {
                            return <span>{section.crn}</span>;
                        })}
                    </Stack>
                </Accordion.Panel>
            </Accordion.Item>
    );
}

export default function PlannerCourseList({ courses, addTimeslot, onExpand }: PlannerCourseListProps) {
    const courseLookup = new Map<string, CourseData>();
    courses.forEach(course => courseLookup.set(`${course.subjectId}  ${course.name}`, course));
    
    return (
        <Accordion variant="separated" value={Array.from(courseLookup.keys())} multiple={true} onChange={courses => {
            console.log(courses);
        }}>
            {courses.map(course => <PlannerCourseCard course={course} onExpand={onExpand} addTimeslot={addTimeslot} />)}
        </Accordion>
    );
}