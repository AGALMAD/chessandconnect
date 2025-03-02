import { GameType } from "../../enums/game";
import { playState } from "../../enums/playState";

export interface GameHistory {
    Id: number,
    UserId: number,
    OpponentId: number,
    Duration: number,
    PlayState: playState,
    Game: GameType
}