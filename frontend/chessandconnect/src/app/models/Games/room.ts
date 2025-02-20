import { GameType } from "../../enums/game";
import { ChessGame } from "./chess/chess-game";

export interface Room {
    Player1Id: number,
    Player2Id: number,
    GameType: GameType

}
