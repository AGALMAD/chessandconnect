
import { Point } from "../base/point"
import { PieceColor } from "./Enums/Color"
import { PieceType } from "./Enums/PieceType"

export interface ChessPiece {
    Id:number,
    PieceType: PieceType
    Color: PieceColor,
    Position: Point
}
