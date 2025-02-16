import { ChessPieceColor } from "./Enums/Color"
import { Point } from "../Base/Point"
import { PieceType } from "./Enums/PieceType"

export interface ChessPiece {
    Id:number,
    PieceType: PieceType
    Color: ChessPieceColor,
    Position: Point
}
