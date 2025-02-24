import { Point } from "../base/point";
import { PieceColor } from "../chess/enums/piece-color";



export interface ConnectPiece {
    Id:number,
    Color: PieceColor,
    Position: Point
}
