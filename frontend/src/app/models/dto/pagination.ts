import { GameType } from "../../enums/game";

export interface Pagination {
    UserId: number;
    GameType: GameType
    GamesCuantity: number;
    ActualPage: number;
}