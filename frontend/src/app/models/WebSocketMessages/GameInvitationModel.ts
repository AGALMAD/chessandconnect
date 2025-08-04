import { FriendshipState } from "../../enums/FriendshipState";
import { GameType } from "../../enums/game";

export class GameInvitationModel {
    HostId!: number;
    FriendId!: number;
    State!: FriendshipState;
    Game!: GameType
}