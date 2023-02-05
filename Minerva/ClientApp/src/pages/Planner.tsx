import React from 'react';
import { ScheduleComponent, Day, Week, WorkWeek, Month, Agenda, Inject } from '@syncfusion/ej2-react-schedule';
import {Flex, Text} from '@mantine/core';

import './Planner.css';
import PlannerControl from "../components/PlannerControl";
import {usePlanner} from "../hooks/usePlanner";
import useTerm from "../hooks/useTerm";


const Planner = () => {

    const {term} = useTerm();
    
    const { scheduleData, getCourses } = usePlanner("000000000000000000000000");

    if (!term) {
        return <Text>Loading...</Text>;
    }
    
    const dateHeaderTemplate = ({date}: any) => {
        return <Text fw={400} fz='lg' c='dimmed'>{date.toLocaleDateString('en-US', {weekday: 'long'})}</Text>;
    }
    
    return (
        <Flex>
            <ScheduleComponent
                selectedDate={new Date(2023, 1, 15)}
                eventSettings={{
                    dataSource: scheduleData(),
                }}
                width={'100%'}
                height={'auto'}
                currentView='WorkWeek'
                views={['Day', 'WorkWeek']}
                timezone='America/New_York'
                timeScale={{enable: true, interval: 30, slotCount: 1}}
                workDays={[1, 2, 3, 4, 5, 6]}
                startHour='07:00'
                endHour='20:00'
                showHeaderBar={false}
                showQuickInfo={false}
                readonly={true}
                allowSwiping={false}
                allowKeyboardInteraction={false}
                dateHeaderTemplate={dateHeaderTemplate}
            >
                <Inject services={[Day, WorkWeek]} />
            </ScheduleComponent>
            <PlannerControl initialCourses={getCourses()} term={term} />
        </Flex>
    );
}

export default Planner;