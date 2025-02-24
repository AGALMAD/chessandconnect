import { Point } from "../base/point"
import { PieceColor } from "./enums/pieceColor"
import { PieceType } from "./enums/pieceType"

export interface ChessPiece {
    Id:number,
    PieceType: PieceType
    Color: PieceColor,
    Position: Point
}
