import { PieceColor } from "./Enums/Color"
import { Point } from "../Base/point"
import { PieceType } from "./Enums/PieceType"

export interface ChessPiece {
    Id:number,
    PieceType: PieceType
    Color: PieceColor,
    Position: Point
}
