import { Game } from "./game";

export interface Room {
    id: number,
    Player1Id: number,
    Player2Id: number,
    StartDate: Date,
    Game: Game

}
