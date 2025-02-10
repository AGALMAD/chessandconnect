import { playState } from "../enums/playState";
import { User } from "./dto/user";
import { GameType } from "../enums/game";

export interface Play {
    id: number,
    userId: number,
    opponentId: number,
    startDate: Date,
    endDate: Date,
    playState: playState,
    game: GameType
}
