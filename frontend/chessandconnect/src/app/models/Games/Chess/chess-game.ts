import { GameType } from "../../../enums/game";
import { ChessBoard } from "./chess-board";

export interface ChessGame {
    GameType: GameType,
    Board: ChessBoard
    StartDate:Date

}
