import { Color } from "./Enums/Color"
import { Point } from "../Base/Point"

export interface BasePiece {
    id:number
    color: Color,
    position: Point
}
