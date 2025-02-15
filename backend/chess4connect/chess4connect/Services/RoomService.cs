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
            var roomMessage = new SocketMessage<RoomDto>
            {
                Type = SocketCommunicationType.GAME_START,

                Data = new RoomDto
                {
                    GameType = gameType,
                    Player1Id = socketPlayer1.Id,
                    Player2Id = socketPlayer2.Id,
                }
            };

            string stringRoomMessage = JsonSerializer.Serialize(roomMessage);

            //Envia los mensajes a los jugadores
            if (socketPlayer1 != null)
            {
                await socketPlayer1.SendAsync(stringRoomMessage);
            }

            if (socketPlayer2 != null)
            {
                await socketPlayer2.SendAsync(stringRoomMessage);
            }




        }



    }
}
