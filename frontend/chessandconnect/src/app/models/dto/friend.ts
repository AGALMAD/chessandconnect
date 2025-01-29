import { Play } from "../play";

export interface Friend {
    id: number,
    userName: string,
    avatarImageUrl: string,
    connected: boolean
    plays: Play[]
}
