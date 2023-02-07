import React, {useState} from 'react';
import {Flex, Text} from '@mantine/core';

import './Planner.css';
import PlannerControl, {CourseData} from "./PlannerControl";
import useTerm from "../../hooks/useTerm";
import {usePlanner} from "../../hooks/usePlanner";
import PlannerSchedule from "./PlannerSchedule";
import {ScheduleTimeslot} from "../../models/planner";
import PlannerCourseList from "./PlannerCourseList";


const Planner = () => {
    const {term} = useTerm();
    const { scheduleData, getCourses } = usePlanner("000000000000000000000000");
    
    const [timeslots, setTimeslots] = useState<ScheduleTimeslot[]>(scheduleData());
    const [courseData, setCourseData] = useState<CourseData[]>([]);

    if (!term) {
        return <Text>Loading...</Text>;
    }
    
    const addTimeslot = (timeslot: ScheduleTimeslot) => {
        setTimeslots([...timeslots, timeslot]);
    }
    
    return (
        <Flex>
            <PlannerSchedule timeslots={timeslots} />
            <PlannerControl initialCourses={getCourses()} term={term} onSetCourses={setCourseData} />
            <PlannerCourseList courses={courseData} addTimeslot={addTimeslot}  onExpand={() => {}}/>
        </Flex>
    );
}

export default Planner;