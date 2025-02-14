using chess4connect.DTOs;
using chess4connect.Enums;
using chess4connect.Models.Games;
using chess4connect.Models.Games.Base;
using chess4connect.Models.Games.Chess;
using chess4connect.Models.Games.Chess.Pieces;
using chess4connect.Models.Games.Chess.Pieces.Base;
using chess4connect.Models.Games.Connect;
using chess4connect.Models.SocketComunication.Handlers;
using chess4connect.Models.SocketComunication.MessageTypes;
using System.Text.Json;
using chess4connect.DTOs.Games;

namespace chess4connect.Services
{
    public class RoomService
    {
        private readonly WebSocketNetwork _network;
        private List<object> rooms = new List<object>();

        public RoomService(WebSocketNetwork webSocketNetwork)
        {
            _network = webSocketNetwork;
        }

        public async Task CreateRoomAsync(GameType gamemode, WebSocketHandler player1, WebSocketHandler player2 = null)
        {
            if (gamemode == GameType.Chess)
            {
                var room = new Room<ChessBasePiece>
                {
                    Player1Id = player1.Id,
                    Player2Id = player2?.Id,
                    Game = new Game<ChessBasePiece>
                    {
                        GameType = gamemode,
                        Board = new ChessBoard()
                    }
                };

                rooms.Add(room);
                await SendRoomMessageAsync(room, player1, player2);
            }
            else if (gamemode == GameType.Connect4)
            {
                var room = new Room<BasePiece>
                {
                    Player1Id = player1.Id,
                    Player2Id = player2?.Id,
                    Game = new Game<BasePiece>
                    {
                        GameType = gamemode,
                        Board = new ConnectBoard()
                    }
                };

                rooms.Add(room);
                await SendRoomMessageAsync(room, player1, player2);
            }
        }

        private async Task SendRoomMessageAsync<T>(Room<T> room, WebSocketHandler socketPlayer1, WebSocketHandler socketPlayer2)
        {

            var socketMessage = new SocketMessage<RoomDto>
            {
                Type = SocketCommunicationType.GAME_START,
                Data = new RoomDto
                {
                    Player1Id = room.Player1Id,
                    Player2Id = room.Player2Id,
                    GameType = room.Game.GameType,
                },
            };

            string stringSocketMessage = JsonSerializer.Serialize(socketMessage);


            await socketPlayer1.SendAsync(stringSocketMessage);
            if (socketPlayer2 is not null)
            {
                await socketPlayer2.SendAsync(stringSocketMessage);
            }
        }



        public object GetGameByUserId(int userId)
        {
            foreach (var roomObj in rooms)
            {
                var roomType = roomObj.GetType();
                var player1IdProperty = roomType.GetProperty("Player1Id");
                var player2IdProperty = roomType.GetProperty("Player2Id");

                if (player1IdProperty != null && player2IdProperty != null)
                {
                    var player1Id = player1IdProperty.GetValue(roomObj)?.ToString();
                    var player2Id = player2IdProperty.GetValue(roomObj)?.ToString();

                    if (player1Id == userId.ToString() || player2Id == userId.ToString())
                    {
                        var gameProperty = roomType.GetProperty("Game");
                        return gameProperty?.GetValue(roomObj);
                    }
                }
            }

            return null; 
        }


    }
}
