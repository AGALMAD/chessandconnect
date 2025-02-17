using chess4connect.DTOs.Games;
using chess4connect.Enums;
using chess4connect.Mappers;
using chess4connect.Models.Games.Base;
using chess4connect.Models.Games.Chess.Chess.Pieces;
using chess4connect.Models.Games.Chess.Chess.Pieces.Base;
using chess4connect.Models.Games.Chess.Chess.Pieces.Types;
using chess4connect.Models.SocketComunication.Handlers;
using chess4connect.Models.SocketComunication.MessageTypes;
using chess4connect.Services;
using System.Security.AccessControl;
using System.Text.Json;

namespace chess4connect.Models.Games.Chess.Chess
{
    public class ChessRoom : BaseRoom
    {

        public ChessGame Game { get; set; }

        public  ChessRoom (WebSocketHandler player1Handler, WebSocketHandler? player2Handler, ChessGame game): base(player1Handler, player2Handler)
        {
            Game = game;
        }

        public override async Task SendRoom()
        {

            int player2Id;
            if (Player2Handler == null)
                player2Id = 0;
            else
                player2Id = Player2Handler.Id;

            var roomMessage = new SocketMessage<RoomDto>
            {
                Type = SocketCommunicationType.GAME_START,

                Data = new RoomDto
                {
                    GameType = Game.GameType,
                    Player1Id = Player1Handler.Id,
                    Player2Id = player2Id,
                }
            };

            string stringRoomMessage = JsonSerializer.Serialize(roomMessage);

            await Player1Handler.SendAsync(stringRoomMessage);


            if (Player2Handler != null)
            {
                await Player2Handler.SendAsync(stringRoomMessage);
            }



            await SendBoard();
            await SendMovementsMessageAsync();
        }




        public override async Task SendBoard()
        {
            //Lista de piezas original
            List<ChessBasePiece> pieces = Game.Board.convertBoardToList();

            //Lista de piezas sin  los movimientos básicos
            var roomMessage = new SocketMessage<ChessBoardDto>
            {
                Type = SocketCommunicationType.CHESS_BOARD,

                Data = new ChessBoardDto
                {
                    Pieces = ChessPieceMapper.ToDto(pieces),
                    Turn = Game.Board.Turn,
                    Player1Time = (int)Game.Board.Player1Time.TotalSeconds,
                    Player2Time = (int)Game.Board.Player2Time.TotalSeconds,

                }
            };

            string stringBoardMessage = JsonSerializer.Serialize(roomMessage);
            //Envia los mensajes a los jugadores
            if (Player1Handler != null)
            {
                await Player1Handler.SendAsync(stringBoardMessage);
            }

            if (Player2Handler != null)
            {
                await Player2Handler.SendAsync(stringBoardMessage);
            }
        }

        public async Task SendMovementsMessageAsync()
        {
            Game.Board.GetAllPieceMovements();

            //Lista de piezas sin  los movimientos básicos
            var roomMessage = new SocketMessage<List<ChessPieceMovementDto>>
            {
                Type = SocketCommunicationType.CHESS_MOVEMENTS,

                Data = ChessPieceMovementsMappper.ToDto(Game.Board.ChessPiecesMovements)
            };

            string stringBoardMessage = JsonSerializer.Serialize(roomMessage);

            WebSocketHandler playerSocket = Game.Turn == ChessPieceColor.WHITE ? Player1Handler : Player2Handler;

            //Envia los movimientos al jugador
            if (playerSocket != null)
            {
                await playerSocket.SendAsync(stringBoardMessage);
            }

        }




        public async Task MoveChessPiece(ChessMoveRequest moveRequest)
        {

            if (Game.Board.MovePiece(moveRequest))
            {
                await SendBoard();

                await SendMovementsMessageAsync();

            }


        }

        public override async Task MessageHandler(string message)
        {
            SocketMessage recived = JsonSerializer.Deserialize<SocketMessage>(message);

            switch(recived.Type)
            {
                case SocketCommunicationType.CHESS_MOVEMENTS:
                    ChessMoveRequest request = JsonSerializer.Deserialize<SocketMessage<ChessMoveRequest>>(message).Data;

                    await MoveChessPiece(request);

                    break;


            }


        }
    }
}
