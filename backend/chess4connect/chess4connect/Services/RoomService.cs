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

namespace chess4connect.Services
{
    public class RoomService
    {
        private readonly WebSocketNetwork _network;
        private List<ChessRoom> chessRooms = new List<ChessRoom>();
        private List<ChessRoom<BasePiece>> connectRooms = new List<ChessRoom<BasePiece>>();
        public RoomService(WebSocketNetwork webSocketNetwork)
        {
            _network = webSocketNetwork;
        }

        public async Task CreateRoomAsync(GameType gamemode, WebSocketHandler player1, WebSocketHandler player2 = null)
        {
            if (gamemode == GameType.Chess)
            {
                var room = new ChessRoom<ChessBasePiece>
                {
                    Player1Id = player1.Id,
                    Player2Id = player2?.Id,
                    Game = new Chess<ChessBasePiece>
                    {
                        GameType = gamemode,
                        Board = new ChessBoard()
                    }
                };

                chessRooms.Add(room);
                await SendRoomMessageAsync(room, player1, player2);
            }
            else if (gamemode == GameType.Connect4)
            {
                var room = new ChessRoom<BasePiece>
                {
                    Player1Id = player1.Id,
                    Player2Id = player2?.Id,
                    Game = new Chess<BasePiece>
                    {
                        GameType = gamemode,
                        Board = new ConnectBoard()
                    }
                };

                connectRooms.Add(room);
                await SendRoomMessageAsync(room, player1, player2);
            }
        }

        private async Task SendRoomMessageAsync<T>(ChessRoom<T> room, WebSocketHandler socketPlayer1, WebSocketHandler socketPlayer2)
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



        public ChessRoom<ChessBasePiece> GetChessRoomByUserId(int userId)
        {
            return chessRooms.FirstOrDefault(r => r.Player1Id == userId || r.Player2Id == userId);


        }

        public ChessRoom<BasePiece> GetConnectRoomByUserId(int userId)
        {
            return connectRooms.FirstOrDefault(r => r.Player1Id == userId || r.Player2Id == userId);

        }



    }
}
