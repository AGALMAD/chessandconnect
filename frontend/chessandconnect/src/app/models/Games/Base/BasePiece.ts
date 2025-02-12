import { Color } from "../Chess/Enums/Color"
import { Point } from "./Point"

export interface BasePiece {
    id:number
    color: Color,
    position: Point
}
