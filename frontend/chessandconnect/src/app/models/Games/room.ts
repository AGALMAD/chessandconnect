import { GameType } from "../../enums/game";
import { Game } from "./Game";

export interface Room {
    Player1Id: number,
    Player2Id: number,
    GameType: GameType

}
