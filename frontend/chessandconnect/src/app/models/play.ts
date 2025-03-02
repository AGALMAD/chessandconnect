import { playState } from "../enums/playState";
import { User } from "./dto/user";
import { GameType } from "../enums/game";

export interface Play {
    id: number,
    user: User,
    opponent: User,
    duration: number
    playState: playState,
    game: GameType
}
