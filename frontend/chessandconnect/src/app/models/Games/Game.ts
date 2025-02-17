import { GameType } from "../../enums/game";
import { ChessBoard } from "./Chess/ChessBoard";

export interface Game {
    GameType: GameType,
    Board: ChessBoard
    StartDate:Date

}
