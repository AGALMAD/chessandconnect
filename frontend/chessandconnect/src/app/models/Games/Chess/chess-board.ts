
import { ChessPiece } from "./chess-piece";
import { PieceColor } from "./Enums/piece-color";

export interface ChessBoard {
    Pieces: ChessPiece[],
    Turn: PieceColor,
    Player1Time: number,
    Player2Time: number
}
