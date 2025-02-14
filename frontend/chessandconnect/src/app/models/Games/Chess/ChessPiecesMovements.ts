import { Point } from "../Base/Point"
import { ChessPiece } from "./ChessPiece"

export interface ChessPieceMovements {
    Piece:ChessPiece,
    Movements: Point[]
}
