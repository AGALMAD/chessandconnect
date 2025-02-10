import { GameType } from "../enums/game";

export interface Room {
    id: number,
    Player1Id: number,
    Player2Id: number,
    Game: GameType

}
