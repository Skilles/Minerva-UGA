import {useEffect, useState} from "react";

import {Meeting, Planner, ScheduleTimeslot} from "../models/planner";
import {DateTime, WeekdayNumbers} from "luxon";

// Converts the start time from number of minutes since 12:00 AM to DateTime in Luxon
const convertDayTime = (day: WeekdayNumbers, startTime: number): DateTime => {
    const hour = Math.floor(startTime / 60);
    const minute = startTime % 60;
    return DateTime.local().set({weekday: day, hour, minute});
}

const convertDayFlag = (dayFlag: number): WeekdayNumbers[] => {
    const days: WeekdayNumbers[] = [];
    if (dayFlag & 1) days.push(6);
    if (dayFlag & 2) days.push(1);
    if (dayFlag & 4) days.push(2);
    if (dayFlag & 8) days.push(3);
    if (dayFlag & 16) days.push(4);
    if (dayFlag & 32) days.push(5);
    return days;
}

// Converts the meeting interface into a ScheduleTimeslot interface
const convertMeeting = (meeting: Meeting): ScheduleTimeslot[] => {
    const days = convertDayFlag(meeting.days);
    const timeslots: ScheduleTimeslot[] = [];
    for (const day of days) {
        timeslots.push({
            Id: 0,
            Subject: meeting.building + " " + meeting.room,
            StartTime: convertDayTime(day, meeting.startTime).toJSDate(),
            EndTime: convertDayTime(day, meeting.endTime).toJSDate()
        });
    }
    return timeslots;
}

// Assigns ids to every timeslot after converting them in batches
const convertMeetings = (meetings: Meeting[]): ScheduleTimeslot[] => {
    const timeslots: ScheduleTimeslot[] = [];
    let id = 0;
    for (const meeting of meetings) {
        const converted = convertMeeting(meeting);
        for (const timeslot of converted) {
            timeslot.Id = id++;
        }
        timeslots.push(...converted);
    }
    return timeslots;
}

export const usePlanner = () => {
    const [planner, setPlanner] = useState<Planner | null>(null);
    
    useEffect(() => {
        const fetchPlannerData = async () => {
            const response = await fetch('api/planner');
            const data = await response.json();
            setPlanner(data);
        }
        fetchPlannerData().catch(console.error);
    }, []);
    
    const plannerData = () => {
        
        
    }
    
    const mapBuildings = () => {
        
    }
    
    return { plannerData, mapBuildings };
}