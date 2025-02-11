import { GameType } from "../../enums/game";
import { BaseBoard } from "./Base/BaseBoard";

export interface Game {
    GameType: GameType,
    Board: BaseBoard
    StartDate:Date

}
