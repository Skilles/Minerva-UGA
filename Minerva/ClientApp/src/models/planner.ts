
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
    sections: number[];
}

export interface Planner {
    name: string;
    term: string;
    courses: Record<string, Course>; // CourseId -> Course
    sections: Record<number, Section>; // CRN -> Section
}

export interface Subject {
    subjectId: string;
    name: string;
}