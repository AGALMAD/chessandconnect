import { FriendshipState } from "../../enums/FriendshipState";
import { Game } from "../game";

export class GameInvitationModel {
    HostId!: number;
    FriendId!: number;
    State!: FriendshipState;
    Game!: Game
}