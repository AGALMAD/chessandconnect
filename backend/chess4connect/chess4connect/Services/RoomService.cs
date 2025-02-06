using chess4connect.Enums;
using chess4connect.Models.Database.Entities;
using chess4connect.Models.SocketComunication.Handlers;
using chess4connect.Models.SocketComunication.MessageTypes;
using System.Text.Json;

namespace chess4connect.Services
{
    public class RoomService
    {

        private readonly WebSocketNetwork _network;

        List<Room> rooms = new List<Room>();
        public RoomService(WebSocketNetwork webSocketNetwork)
        {
            _network = webSocketNetwork;
        }


        public async Task AddToRoom(Game gamemode, WebSocketHandler player1, WebSocketHandler player2)
        {
            switch (gamemode)
            {
                case Game.Chess:
                    Room roomChess = new Room
                    {
                        Player1 = player1,
                        Player2 = player2,
                        StartDate = DateTime.Now,
                        Game = Enums.Game.Chess
                    };

                    rooms.Add(roomChess);
                    await SendRoomMessage(roomChess, player1, player2);
                    break;

                case Game.Connect4:
                    Room roomConnect = new Room
                    {
                        Player1 = player1,
                        Player2 = player2,
                        StartDate = DateTime.Now,
                        Game = Enums.Game.Connect4
                    };

                    rooms.Add(roomConnect);
                    await SendRoomMessage(roomConnect, player1, player2);
                    break;
            }


        }

        private async Task SendRoomMessage(Room room, WebSocketHandler player1, WebSocketHandler player2)
        {
            var roomSocketMessage = new SocketMessage<Room>
            {
                Type = Enums.SocketCommunicationType.GAME_START,
                Data = room
            };

            string message = JsonSerializer.Serialize(roomSocketMessage);

            await player1.SendAsync(message);
            await player2.SendAsync(message);
        }


        

    }
}
