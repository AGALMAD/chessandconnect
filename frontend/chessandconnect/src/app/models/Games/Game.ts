import { GameType } from "../../enums/game";
import { BaseBoard } from "./Chess/ChessBoard";

export interface Game {
    GameType: GameType,
    Board: BaseBoard
    StartDate:Date

}
