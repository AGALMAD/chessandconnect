import { Color } from "./Enums/Color"
import { Point } from "../Base/Point"
import { PieceType } from "./Enums/PieceType"

export interface BasePiece {
    id:number,
    pieceType: PieceType
    color: Color,
    position: Point
}
