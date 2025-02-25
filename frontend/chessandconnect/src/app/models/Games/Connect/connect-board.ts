
import { ConnectPiece } from "./connect-piece"


export interface ConnectBoard {
    LastPiece: ConnectPiece
    Player1Turn: boolean
    Player1Time: number
    Player2Time: number
}
