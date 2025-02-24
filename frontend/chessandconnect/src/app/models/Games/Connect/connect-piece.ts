import { Point } from "../base/point";
import { PieceColor } from "../chess/Enums/piece-color";

export interface ConnectPiece {
    Id:number,
    Color: PieceColor,
    Position: Point
}
