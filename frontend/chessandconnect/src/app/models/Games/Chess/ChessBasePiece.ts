import { ChessPieceColor } from "./Enums/Color"
import { Point } from "../Base/Point"
import { PieceType } from "./Enums/PieceType"

export interface ChessBasePiece {
    Id:number,
    Color: ChessPieceColor,
    Position: Point
    PieceType: PieceType
    PossibleMovements: Point[]
}
