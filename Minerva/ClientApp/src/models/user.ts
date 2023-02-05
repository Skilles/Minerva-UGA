

interface UserData {
    planners: string[];
}

export interface User {
    id: string;
    firstName: string;
    lastName: string;
    email: string;
    role: string;
    authToken?: string;
    data: UserData;
}