using chess4connect.DTOs;
using chess4connect.Enums;
using chess4connect.Models.Games.Base;
using chess4connect.Models.Games.Chess.Chess;
using chess4connect.Models.Games.Chess.Chess.Pieces;
using chess4connect.Models.Games.Chess.Chess.Pieces.Base;
using chess4connect.Models.Games.Connect;
using chess4connect.Models.SocketComunication.Handlers;
using chess4connect.Models.SocketComunication.MessageTypes;
using System.Text.Json;
using chess4connect.DTOs.Games;
using chess4connect.Models.Games.Chess;

namespace chess4connect.Services
{
    public class RoomService
    {
        private readonly WebSocketNetwork _network;
        private List<ChessRoom> chessRooms = new List<ChessRoom>();
        private List<ConnectRoom> connectRooms = new List<ConnectRoom>();
        public RoomService(WebSocketNetwork webSocketNetwork)
        {
            _network = webSocketNetwork;
        }

        public async Task CreateRoomAsync(GameType gamemode, WebSocketHandler player1, WebSocketHandler player2 = null)
        {
            if (gamemode == GameType.Chess)
            {
                var room = new ChessRoom(player1.Id, player2.Id,
                    new ChessGame(DateTime.Now,
                    new ChessBoard()));

                chessRooms.Add(room);

                await SendRoomMessageAsync(GameType.Chess, player1, player2);
            }
            else if (gamemode == GameType.Connect4)
            {
                var room = new ConnectRoom(player1.Id, player2.Id,
                   new ConnectGame(DateTime.Now,
                   new ConnectBoard()));

                connectRooms.Add(room);

                await SendRoomMessageAsync(GameType.Connect4, player1, player2);
            }
        }

        private async Task SendRoomMessageAsync(GameType gameType, WebSocketHandler socketPlayer1, WebSocketHandler socketPlayer2)
        {
            string stringMessage = "";

            if (gameType == GameType.Chess)
            {

                //Mensaje para los jugadores de ajedrez

                var chessBoardMessage = new SocketMessage<List<ChessBasePiece>>
                {
                    Type = SocketCommunicationType.CHESS_BOARD,

                    Data = new List<ChessBasePiece>()
                };

                stringMessage = JsonSerializer.Serialize(chessBoardMessage);
            }
            else if (gameType == GameType.Connect4)
            {
                //Mensaje para los jugadores de conecta4
                var connectBoardMessage = new SocketMessage<List<BasePiece>>
                {
                    Type = SocketCommunicationType.CONNECT_BOARD,

                    Data = new List<BasePiece>()
                };
                stringMessage = JsonSerializer.Serialize(connectBoardMessage);
            }

            //Envia los mensajes a los jugadores
            if (socketPlayer1 != null)
            {
                await socketPlayer1.SendAsync(stringMessage);
            }

            if (socketPlayer2 != null)
            {
                await socketPlayer2.SendAsync(stringMessage);
            }




        }



    }
}
