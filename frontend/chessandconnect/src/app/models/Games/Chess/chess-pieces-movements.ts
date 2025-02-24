import { Point } from "../base/point"
import { ChessPiece } from "./chess-piece"

export interface ChessPieceMovements {
    Id: number,
    Movements: Point[]
}
