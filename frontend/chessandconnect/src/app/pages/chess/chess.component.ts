import { Component, OnDestroy, OnInit, Type } from '@angular/core';
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

import { PieceType } from '../../enums/piece-type';
import { ChessPiece } from '../../models/Games/Chess/chess-piece';
import { ChessMoveRequest } from '../../models/Games/Chess/chess-move-request';
import { MatchMakingService } from '../../services/match-making.service';
import { Router } from '@angular/router';





@Component({
  selector: 'app-chess',
  imports: [CommonModule, ChatComponent, PipeTimerPipe],
  templateUrl: './chess.component.html',
  styleUrl: './chess.component.css'
})

export class ChessComponent implements OnInit,OnDestroy {

  public baseUrl = environment.apiUrl;

  selectedPiece: ChessPiece | null = null;

  constructor(
    private websocketService:WebsocketService, 
    public gameService: GameService, 
    public authService : AuthService,
    public chessService: ChessService,    
  ) { }


  ngOnInit(): void {
    console.log("OPPONENT", this.gameService.opponent)
  }


  ngOnDestroy(): void {

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


/*
<div class="h-screen w-screen flex flex-col sm:flex-row items-center justify-center bg-brown-700">

  <!-- Contenedor del tablero -->
  <div class="relative flex flex-col items-center justify-center w-full h-full sm:w-[90vh] sm:h-[90vh]">

    <div class="mb-6 flex items-center justify-between w-auto sm:w-[80vh] mx-auto space-x-10">

      <!-- Avatar y nombre del oponente -->
      <div class="flex items-center space-x-3">
        <img *ngIf="gameService.opponent?.avatarImageUrl" [src]="baseUrl + gameService.opponent.avatarImageUrl"
          alt="Opponent" class="w-10 h-10 sm:w-12 sm:h-12 rounded-full border-2 border-black">
        <p class="text-[#301e16] font-bold text-sm sm:text-base">{{ gameService.opponent?.userName || 'Oponente' }}</p>
      </div>
    
      <!-- Temporizador del oponente -->
      <div class="bg-[#737171] text-white rounded-md p-2 text-sm sm:text-xl font-bold">
        {{ gameService.opponentTimer | pipeTimer}}
      </div>
    
    </div>
    
    
    <!--Tablero normal-->
    @if (gameService.playerColor) {
      <div class="w-full h-full flex justify-center items-center">
        <!-- Contenedor del tablero siempre cuadrado -->
        <div class="relative w-full h-full max-w-[90vw] max-h-[90vw] sm:max-w-[80vh] sm:max-h-[80vh] shadow-2xl rounded-lg border-4 border-[#D4A373] bg-[#D4A373] overflow-hidden" style="aspect-ratio: 1 / 1;">
          
          <!-- Tablero -->
          <div class="grid grid-cols-8 grid-rows-8 gap-0 absolute top-0 left-0 w-full h-full rounded-md overflow-hidden">
            
            <!-- Fila y columna del tablero -->
            @for (row of rows; track $index; let i = $index) {
            @for (col of cols; track $index; let j = $index) {
              <div class="relative w-full h-full flex items-center justify-center transition-all duration-200"
                [class.bg-brown-200]="(i + j) % 2 === 0"
                [class.bg-brown-600]="(i + j) % 2 !== 0"
                (click)="movePiece(i, j)">
                
                <!-- Coordenadas en la primera columna -->
                <span *ngIf="j === 0" class="absolute top-0 left-0 text-xs sm:text-sm text-gray-800 font-semibold p-1">
                  {{ 8 - i }}
                </span>
      
                <!-- Coordenadas en la fila inferior (letras) a la derecha -->
                <span *ngIf="i === 7" class="absolute bottom-0 right-0 text-xs sm:text-sm text-gray-800 font-semibold p-1">
                  {{ letters[j] }}
                </span>
      
                @for (piece of chessService.pieces; track $index) {
                  @if (piece.Position.X === i && piece.Position.Y === j) {
                    @if (!piece.Player1Piece) {
                      <span class="cursor-pointer text-xl sm:text-2xl md:text-3xl lg:text-4xl xl:text-5xl text-black transition-transform hover:scale-110">
                        {{getPieceSymbol(piece.PieceType)}}
                      </span>
                    }
                    @else {
                      <span (click)="selectPiece(piece)" class="cursor-pointer text-xl sm:text-2xl md:text-3xl lg:text-4xl xl:text-5xl text-white transition-transform hover:scale-110">
                        {{getPieceSymbol(piece.PieceType)}}
                      </span>
                    }
                  }
                }
      
                <!-- Mostrar movimientos posibles -->
                @if (chessService.showMovements != null) {
                  @for (movements of chessService.showMovements.Movements; track $index) {
                    @if (movements.X == i && movements.Y == j) {
                      <div class="cursor-pointer flex items-center justify-center absolute inset-0 z-40" (click)="movePiece(i, j)">
                        <div class="w-6 h-6 sm:w-8 sm:h-8 md:w-10 md:h-10 lg:w-12 lg:h-12 rounded-full bg-white opacity-75 hover:opacity-100 transition-opacity duration-200"></div>
                      </div>
                    }
                  }
                }
              </div>
            }
            }
          </div>
      
        </div>
      </div>
      
    }
    <!-- tablero invertido -->
    @else {
      <div class="w-full h-full flex justify-center items-center">
        <!-- Contenedor del tablero siempre cuadrado -->
        <div class="relative w-full h-full max-w-[90vw] max-h-[90vw] sm:max-w-[80vh] sm:max-h-[80vh] shadow-2xl rounded-lg border-4 border-[#D4A373] bg-[#D4A373] overflow-hidden" style="aspect-ratio: 1 / 1;">
          
          <!-- Tablero -->
          <div class="grid grid-cols-8 grid-rows-8 gap-0 absolute top-0 left-0 w-full h-full rounded-md overflow-hidden">
            
            <!-- Fila y columna del tablero -->
            @for (row of rows; track $index; let i = $index) {
            @for (col of cols; track $index; let j = $index) {
              <div class="relative w-full h-full flex items-center justify-center transition-all duration-200"
                [class.bg-brown-200]="(i + j) % 2 !== 0"
                [class.bg-brown-600]="(i + j) % 2 === 0"
                [class.bg-transparent]="true" 
                (click)="movePiece(row, col)"> <!-- Coordenadas invertidas en clic -->
                
                <!-- Coordenadas en la primera columna -->
                <span *ngIf="j === 0" class="absolute top-0 left-0 text-xs sm:text-sm text-gray-800 font-semibold p-1">
                  {{ i + 1 }} <!-- Cambiado para que muestre el número correcto -->
                </span>
      
                <!-- Coordenadas en la fila inferior (letras) a la derecha -->
                <span *ngIf="i === 7" class="absolute bottom-0 right-0 text-xs sm:text-sm text-gray-800 font-semibold p-1">
                  {{ letters[7 - j] }} <!-- Ajustado para invertir letras -->
                </span>
      
                <!-- Dibujar piezas en la nueva ubicación -->
                @for (piece of chessService.pieces; track $index) {
                  @if (piece.Position.X === row && piece.Position.Y === col) { 
                    @if (!piece.Player1Piece) {
                      <span (click)="selectPiece(piece)" class="cursor-pointer text-xl sm:text-2xl md:text-3xl lg:text-4xl xl:text-5xl text-black transition-transform hover:scale-110">
                        {{ getPieceSymbol(piece.PieceType) }}
                      </span>
                    }
                    @else {
                      <span class="cursor-pointer text-xl sm:text-2xl md:text-3xl lg:text-4xl xl:text-5xl text-white transition-transform hover:scale-110">
                        {{ getPieceSymbol(piece.PieceType) }}
                      </span>
                    }
                  }
                }
      
                <!-- Mostrar movimientos posibles -->
                <!-- Mostrar movimientos posibles -->
                @if (chessService.showMovements != null) {
                  @for (movements of chessService.showMovements.Movements; track $index) {
                    @if (movements.X == row && movements.Y == col) {
                      <div class="cursor-pointer flex items-center justify-center absolute inset-0 z-40" (click)="movePiece(row, col)">
                        <div class="w-6 h-6 sm:w-8 sm:h-8 md:w-10 md:h-10 lg:w-12 lg:h-12 rounded-full bg-white opacity-75 hover:opacity-100 transition-opacity duration-200"></div>
                      </div>
                    }
                  }
                }
              </div>
            }
            }
          </div>
      
        </div>
      </div>
      
    }

    <!-- Jugador -->
    <div class="mt-6 flex items-center justify-between w-auto sm:w-[80vh] mx-auto space-x-10">

      <!-- Avatar y nombre del jugador -->
      <div class="flex items-center space-x-3">
        <img [src]="baseUrl + authService.currentUser?.avatarImageUrl" alt="Player"
          class="w-10 h-10 sm:w-12 sm:h-12 rounded-full border-2 border-black">
        <p class="text-[#301e16] font-bold text-sm sm:text-base">{{ authService.currentUser?.userName || 'Jugador' }}</p>
      </div>
    
      <!-- Temporizador del jugador -->
      <div class="bg-[#b8b5b5] text-white rounded-md p-2 text-sm sm:text-xl font-bold">
        {{ gameService.currentPlayerTimer | pipeTimer }}
      </div>
    
    </div>
    

  </div>
    <div class="w-full sm:w-1/4 h-auto sm:h-full flex items-center justify-center mt-4 sm:mt-0">
      <app-chat></app-chat>
    </div>
</div> 
*/