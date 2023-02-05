import React from 'react';
import { ScheduleComponent, Day, Week, WorkWeek, Month, Agenda, Inject } from '@syncfusion/ej2-react-schedule';
import { Text } from '@mantine/core';

import './Planner.css';

const Planner = () => {

    const data = [
        {
            Id: 1,
            Subject: 'Meeting',
            StartTime: new Date(2023, 1, 15, 10, 0),
            EndTime: new Date(2023, 1, 15, 12, 30)
        },
    ];
    
    const dateHeaderTemplate = ({date}: any) => {
        return <Text fw={400} fz='lg' c='dimmed'>{date.toLocaleDateString('en-US', {weekday: 'long'})}</Text>;
    }
    
    
    return (
        // hide the day of the month at the top
        <ScheduleComponent
            selectedDate={new Date(2023, 1, 15)}
            eventSettings={{
                dataSource: data,
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
    );
}

export default Planner;