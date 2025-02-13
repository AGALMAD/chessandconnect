import { Game } from "./Game";

export interface Room<T> {
    Player1Id: number;
    Player2Id?: number;
    Game: Game<T>;
}