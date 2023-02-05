
export interface ScheduleTimeslot {
    Id: number;
    Subject: string;
    StartTime: Date;
    EndTime: Date;
}

export interface Meeting {
    building: string;
    room: string;
    startTime: number;
    endTime: number;
    days: number;
}

export interface Section {
    professor: string;
    meetings: Meeting[];
    crn: number;
    credits: number;
}

export interface Course {
    name: string;
    sections: Section[];
}

export interface Planner {
    name: string;
    courses: Course[];
}