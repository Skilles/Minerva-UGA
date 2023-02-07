import {Day, Inject, ScheduleComponent, WorkWeek} from "@syncfusion/ej2-react-schedule";
import React from "react";
import {ScheduleTimeslot} from "../../models/planner";
import {Text} from "@mantine/core";


interface PlannerScheduleProps {
    timeslots: ScheduleTimeslot[];
}

export default function PlannerSchedule({ timeslots }: PlannerScheduleProps) {

    const dateHeaderTemplate = ({date}: any) => {
        return <Text fw={400} fz='lg' c='dimmed'>{date.toLocaleDateString('en-US', {weekday: 'long'})}</Text>;
    }
    
    return (
        <ScheduleComponent
            selectedDate={new Date(2023, 1, 15)}
            eventSettings={{
                dataSource: timeslots,
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