import { PieceColor } from "./Enums/piece-color"
import { PieceType } from "./Enums/piece-type"
import { Point } from "../base/point"

export interface ChessPiece {
    Id:number,
    PieceType: PieceType
    Color: PieceColor,
    Position: Point
}
