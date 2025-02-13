import { GameType } from "../../enums/game";
import { Board } from "./Base/Board";

export interface Game<T> {
    GameType: GameType;
    Board: Board<T>;
}