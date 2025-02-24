import { PieceColor } from "./Enums/piece-color";
import { ChessPiece } from "./chess-piece";

export interface ChessBoard {
    Pieces: ChessPiece[],
    Turn: PieceColor,
    Player1Time: number,
    Player2Time: number
}
