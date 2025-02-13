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
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

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

        private async Task SendRoomMessageAsync<T>(Room<T> room, WebSocketHandler player1, WebSocketHandler player2)
        {


            var roomSocketMessage = new SocketMessage<Room<T>>
            {
                Type = SocketCommunicationType.GAME_START,
                Data = room,
            };

            string message = JsonSerializer.Serialize(roomSocketMessage);

            await player1.SendAsync(message);
            if (player2 is not null)
            {
                await player2.SendAsync(message);
            }
        }
    }
}
