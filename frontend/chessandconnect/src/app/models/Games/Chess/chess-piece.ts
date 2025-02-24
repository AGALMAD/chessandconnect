import { Point } from "../base/point"
import { PieceColor } from "./enums/piece-color"
import { PieceType } from "./enums/piece-type"


export interface ChessPiece {
    Id:number,
    PieceType: PieceType
    Color: PieceColor,
    Position: Point
}
