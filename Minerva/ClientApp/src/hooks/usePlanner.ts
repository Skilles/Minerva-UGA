import {useEffect, useState} from "react";

import {Course, Meeting, Planner, ScheduleTimeslot, Subject} from "../models/planner";
import {DateTime, WeekdayNumbers} from "luxon";
import {useAuth} from "./useAuth";
import useFetch from "./useFetch";

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

export const usePlanner = (plannerId: string) => {
    const { isLoggedIn} = useAuth();
    const { fetchWithAuth } = useFetch();
    
    const [planner, setPlanner] = useState<Planner | null>(null);

    useEffect(() => {
        if (!isLoggedIn) {
            return;
        }
        fetchWithAuth(`planner?id=${plannerId}`).then(data => {
            setPlanner(data);
        }).catch(console.error);
        
    }, []);
    
    const getSection = (crn: number) => {
        if (!planner) return null;
        return planner.sections[crn];
    }
    
    const scheduleData = (): ScheduleTimeslot[] => {
        if (!planner) return [];
        const meetings: Meeting[] = [];
        for (const course of Object.values(planner.courses)) {
            for (const crn of course.sections) {
                const section = planner.sections[crn];
                if (section) {
                    meetings.push(...section.meetings);
                }
            }
        }
        return convertMeetings(meetings);
    }
    
    // Get all building names for all the meetings on a certain day
    const buildingsData = (day: number): string[] => {
        if (!planner) return [];
        const buildings: string[] = [];
        for (const section of Object.values(planner.sections)) {
            for (const meeting of section.meetings) {
                if (meeting.days & (1 << day)) {
                    buildings.push(meeting.building);
                }
            }
        }
        return buildings;
    }
    
    const getCourses = (): Course[] => {
        if (!planner) return [];
        return Object.values(planner.courses);
    }
    
    return { scheduleData, buildingsData, getCourses };
}