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
        private readonly IServiceScopeFactory _scopeFactory;


        private List<ChessRoom> chessRooms = new List<ChessRoom>();
        private List<ConnectRoom> connectRooms = new List<ConnectRoom>();

        public async Task CreateRoomAsync(GameType gamemode, WebSocketHandler player1Hadler, WebSocketHandler player2Handler = null)
        {

            if (gamemode == GameType.Chess)
            {
                var room = new ChessRoom(player1Hadler, player2Handler,
                    new ChessGame(DateTime.Now,
                    new ChessBoard()
                    {
                        StartTurnDateTime = DateTime.Now,
                    }));


                chessRooms.Add(room);

                await room.SendRoom();

            }
            else if (gamemode == GameType.Connect4)
            {
                var room = new ConnectRoom(player1Hadler, player2Handler,
                   new ConnectGame(DateTime.Now,
                   new ConnectBoard()));

                connectRooms.Add(room);

                await room.SendRoom();

            }

        }

        public async Task MessageHandler(int userId, string message)
        {

            await GetChessRoomByUserId(userId).MessageHandler(message);

        }



        public ChessRoom GetChessRoomByUserId(int userId)
        {
            return chessRooms.FirstOrDefault(r => r.Player1Handler.Id == userId || r.Player2Handler.Id == userId);
        }
        public ConnectRoom GetConnectRoomByUserId(int userId)
        {
            return connectRooms.FirstOrDefault(r => r.Player1Handler.Id == userId || r.Player2Handler.Id == userId);
        }





    }
}
