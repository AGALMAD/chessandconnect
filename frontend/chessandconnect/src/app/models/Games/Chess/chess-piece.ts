import { Point } from "../Base/Point"
import { PieceType } from "../../../enums/piece-type"

export interface ChessPiece {
    Id:number,
    PieceType: PieceType
    Player1Piece: boolean,
    Position: Point
}
