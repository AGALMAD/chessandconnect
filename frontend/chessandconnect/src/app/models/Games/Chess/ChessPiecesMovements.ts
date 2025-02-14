import { Point } from "../Base/Point"
import { ChessBasePiece } from "./ChessPiece"

export interface ChessPieceMovements {
    Piece:ChessBasePiece,
    Movements: Point[]
}
