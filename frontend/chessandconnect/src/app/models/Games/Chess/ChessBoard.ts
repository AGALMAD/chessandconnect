import { ChessPiece } from "./ChessPiece";
import { ChessPieceColor } from "./Enums/Color";

export interface ChessBoard {
    Pieces: ChessPiece[],
    Turn: ChessPieceColor,
    Player1Time: number,
    Player2Time: number
}
