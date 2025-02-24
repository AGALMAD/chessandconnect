import { ChessPiece } from "./chess-piece";


export interface ChessBoard {
    Pieces: ChessPiece[],
    Player1Turn: boolean,
    Player1Time: number,
    Player2Time: number
}
