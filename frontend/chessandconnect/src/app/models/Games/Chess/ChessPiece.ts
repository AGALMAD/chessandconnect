import { ChessPieceColor } from "./Enums/Color"
import { Point } from "../Base/Point"
import { PieceType } from "./Enums/PieceType"

export interface ChessBasePiece {
    Id:number,
    PieceType: PieceType
    ChessPieceColor: ChessPieceColor,
    Position: Point
}
