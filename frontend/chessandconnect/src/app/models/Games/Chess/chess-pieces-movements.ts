import { Point } from "../Base/point"
import { ChessPiece } from "./chess-piece"

export interface ChessPieceMovements {
    Id: number,
    Movements: Point[]
}
