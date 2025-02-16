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
        private readonly IServiceScopeFactory _scopeFactory;


        private List<ChessRoom> chessRooms = new List<ChessRoom>();
        private List<ConnectRoom> connectRooms = new List<ConnectRoom>();
        public RoomService(WebSocketNetwork webSocketNetwork, IServiceScopeFactory scopeFactory)
        {
            _network = webSocketNetwork;
            _scopeFactory = scopeFactory;

        }

        public async Task CreateRoomAsync(GameType gamemode, WebSocketHandler player1, WebSocketHandler player2 = null)
        {
            int player2Id = 0;
            if (player2 == null)
                player2Id = 0;
            else 
                player2Id = player2.Id;

            if (gamemode == GameType.Chess)
            {
                var room = new ChessRoom(player1.Id, player2Id,
                    new ChessGame(DateTime.Now,
                    new ChessBoard()));

                chessRooms.Add(room);

                await SendRoomMessageAsync(GameType.Chess, player1, player2);
            }
            else if (gamemode == GameType.Connect4)
            {
                var room = new ConnectRoom(player1.Id, player2Id,
                   new ConnectGame(DateTime.Now,
                   new ConnectBoard()));

                connectRooms.Add(room);

                await SendRoomMessageAsync(GameType.Connect4, player1, player2);
            }
        }

        private async Task SendRoomMessageAsync(GameType gameType, WebSocketHandler socketPlayer1, WebSocketHandler socketPlayer2 = null)
        {
            int player2Id;
            if (socketPlayer2 == null)
                player2Id = 0;
            else
                player2Id = socketPlayer2.Id;

            var roomMessage = new SocketMessage<RoomDto>
            {
                Type = SocketCommunicationType.GAME_START,

                Data = new RoomDto
                {
                    GameType = gameType,
                    Player1Id = socketPlayer1.Id,
                    Player2Id = player2Id,
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

            //Crea el servicio scoped para poder enviar las fichas del tablero
            using (var scope = _scopeFactory.CreateScope())
            {
                var gameService = scope.ServiceProvider.GetRequiredService<GameService>();
                await gameService.SendBoardMessageAsync(socketPlayer1.Id, player2Id, gameType);
                await gameService.SendMovementsMessageAsync(socketPlayer1.Id);
            }
        }


        public ChessRoom GetChessRoomByUserId(int userId)
        {
            return chessRooms.FirstOrDefault(r => r.Player1Id == userId || r.Player2Id == userId);
        }
        public ChessRoom GetConnectRoomByUserId(int userId)
        {
            return chessRooms.FirstOrDefault(r => r.Player1Id == userId || r.Player2Id == userId);
        }





    }
}
