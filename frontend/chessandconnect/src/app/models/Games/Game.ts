import { GameType } from "../../enums/game";
import { BaseBoard } from "./Base/BaseBoard";
import { BasePiece } from "./Base/BasePiece";

export interface Game {
    GameType: GameType,
    Pieces: BasePiece[]
    StartDate:Date

}
