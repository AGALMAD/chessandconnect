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

            }
            else if (gamemode == GameType.Connect4)
            {
                var room = new ConnectRoom(player1Hadler, player2Handler,
                   new ConnectGame(DateTime.Now,
                   new ConnectBoard()));

                connectRooms.Add(room);

            }

            await SendRoomMessageAsync(gamemode, player1Hadler, player2Handler);
        }

        private async Task SendRoomMessageAsync(GameType gameType, WebSocketHandler socketPlayer1, WebSocketHandler socketPlayer2 = null)
        {
            ChessRoom room = GetChessRoomByUserId(socketPlayer1.Id);

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
                //Envia el mensaje del tablero a los dos jugadores
                var gameService = scope.ServiceProvider.GetRequiredService<GameService>();
                await gameService.SendChessBoardMessageAsync(GetChessRoomByUserId(socketPlayer1.Id), socketPlayer1, socketPlayer2);

                //Envia el mensaje de los movimientos al jugador 1
                await gameService.SendMovementsMessageAsync(room, socketPlayer1);
            }
        }


        public ChessRoom GetChessRoomByUserId(int userId)
        {
            return chessRooms.FirstOrDefault(r => r.Player1Id == userId || r.Player2Id == userId);
        }
        public ConnectRoom GetConnectRoomByUserId(int userId)
        {
            return connectRooms.FirstOrDefault(r => r.Player1Id == userId || r.Player2Id == userId);
        }





    }
}
