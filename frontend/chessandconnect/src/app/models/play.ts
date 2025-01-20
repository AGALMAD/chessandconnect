import { playState } from "../enums/playState";
import { User } from "./dto/user";
import { Game } from "./game";

export interface Play {
    id: number,
    user: User,
    opponent: User,
    startDate: Date,
    endDate: Date,
    playState: playState,
    game: Game
}
