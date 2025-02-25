import { Point } from "../Base/Point"
import { PieceType } from "../../../enums/piece-type"

export interface ChessPiece {
    Id:number,
    Player1Piece: boolean,
    Position: Point
    PieceType: PieceType
}
