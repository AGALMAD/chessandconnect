import { Point } from "../Base/point";
import { PieceColor } from "../Chess/Enums/Color";

export interface ConnectPiece {
    Id:number,
    Color: PieceColor,
    Position: Point
}
