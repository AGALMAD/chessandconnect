import { FriendshipState } from "../../enums/FriendshipState";

export class GameInvitationModel {
    HostId!: number;
    FriendId!: number;
    State!: FriendshipState;

}