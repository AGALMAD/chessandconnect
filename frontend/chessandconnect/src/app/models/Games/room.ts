import { GameType } from "../../enums/game";
import { ChessGame } from "./Chess/chess-game";

export interface Room {
    Player1Id: number,
    Player2Id: number,
    GameType: GameType

}
