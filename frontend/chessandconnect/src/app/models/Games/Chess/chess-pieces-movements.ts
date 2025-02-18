import { Point } from "../Base/point"
import { ChessPiece } from "./chess-piece"

export interface ChessPieceMovements {
    Piece:ChessPiece,
    Movements: Point[]
}
