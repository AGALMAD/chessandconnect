import { Point } from "../base/point"
import { PieceType } from "./enums/piece-type"
import { PieceColor } from "./enums/pieceColor"

export interface ChessPiece {
    Id:number,
    PieceType: PieceType
    Color: PieceColor,
    Position: Point
}
