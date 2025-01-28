import { Play } from "../play";

export interface Friend {
    id: number,
    userName: string,
    avatarImageUrl: string,
    plays: Play[]
}
