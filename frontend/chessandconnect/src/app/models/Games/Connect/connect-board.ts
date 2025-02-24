

import { PieceColor } from "../chess/enums/piece-color";
import { ConnectPiece } from "./connect-piece";

export interface ConnectBoard {
    Pieces: ConnectPiece[]
    Turn: PieceColor
    Player1Time: number
    Player2Time: number
}
