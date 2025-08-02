import { FriendshipState } from "../../enums/FriendshipState"

export interface Friendship {
    Id: number
    UserId: number
    FriendId: number
    State: FriendshipState
}
