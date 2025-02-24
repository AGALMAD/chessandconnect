import { Component, OnInit, Type } from '@angular/core';
import { GameService } from '../../services/game.service';
import { CommonModule } from '@angular/common';
import { ApiService } from '../../services/api.service';
import { environment } from '../../../environments/environment';
import { AuthService } from '../../services/auth.service';
import { WebsocketService } from '../../services/websocket.service';
import { SocketMessage, SocketMessageGeneric } from '../../models/WebSocketMessages/SocketMessage';
import { SocketCommunicationType } from '../../enums/SocketCommunicationType';
import { ChatComponent } from "../../components/chat/chat.component";
import { ChessService } from '../../services/chess.service';
import { PipeTimerPipe } from '../../pipes/pipe-timer.pipe';
import { PieceType } from '../../models/Games/Chess/Enums/piece-type';
import { ChessPiece } from '../../models/Games/Chess/chess-piece';
import { ChessMoveRequest } from '../../models/Games/Chess/chess-move-request';





@Component({
  selector: 'app-chess',
  imports: [CommonModule, ChatComponent, PipeTimerPipe],
  templateUrl: './chess.component.html',
  styleUrl: './chess.component.css'
})

export class ChessComponent implements OnInit {

  public baseUrl = environment.apiUrl;

  selectedPiece: ChessPiece | null = null;

  constructor(
    private websocketService:WebsocketService, 
    public gameService: GameService, 
    private api: ApiService, 
    public authService : AuthService,
    public chessService: ChessService
  ) { }


  ngOnInit(): void {
    console.log("Opponent:", this.gameService.opponent)
    console.log("PIECES:", this.chessService.pieces)
  }





  letters: string[] = ['A', 'B', 'C', 'D', 'E', 'F', 'G', 'H']
  lettersReverse: string[] = ['H', 'G', 'F', 'E', 'D', 'C', 'B', 'A']
  numbersReverse: string[] = ['1', '2', '3', '4', '5', '6', '7', '8']
  numbers: string[] = ['8', '7', '6', '5', '4', '3', '2', '1']

  rows: number[] = [7, 6, 5, 4, 3, 2, 1, 0]
  cols: number[] = [0, 1, 2, 3, 4, 5, 6, 7]
  cells: string[] = [];



  getPieceSymbol(pieceType: PieceType): string {
    switch (pieceType) {
      case PieceType.BISHOP: return '♝';
      case PieceType.KING: return '♚';
      case PieceType.KNIGHT: return '♞';
      case PieceType.PAWN: return '♙';
      case PieceType.QUEEN: return '♛';
      case PieceType.ROOK: return '♜';
      default: return '';
    }
  }

  selectPiece(piece: ChessPiece) {
    this.selectedPiece = piece;
    console.log(piece)
  
    this.chessService.showMovements = this.chessService.movements.find(m => m.Id == piece.Id) || null;
  }
  
  async movePiece(destinationX: number, destinationY: number) {
    if (!this.selectedPiece) {
      return;
      
    }

    const moveRequest: ChessMoveRequest = { PieceId: this.selectedPiece.Id, MovementX: destinationX,  MovementY: destinationY};

    const message : SocketMessageGeneric<ChessMoveRequest> = {
      Type : SocketCommunicationType.CHESS_MOVEMENTS,
      Data : moveRequest
    }

    this.websocketService.sendRxjs(JSON.stringify(message))

  }







}