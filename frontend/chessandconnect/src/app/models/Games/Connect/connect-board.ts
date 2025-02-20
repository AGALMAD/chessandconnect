import { PieceColor } from "../Chess/Enums/Color";
import { ConnectPiece } from "./connect-piece";

export interface ConnectBoard {
    Pieces: ConnectPiece[]
    Turn: PieceColor
    Player1Time: number
    Player2Time: number
}
