import { Play } from "../play";

export interface User {
    id: number,
    userName: string,
    email: string,
    avatarImageUrl: string,
    plays: Play[]
}