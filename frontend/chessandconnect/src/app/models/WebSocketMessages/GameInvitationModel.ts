import { FriendshipState } from "../../enums/FriendshipState";

export class GameInvitationModel {
    UserId!: number;
    FriendId!: number;
    State!: FriendshipState;

}