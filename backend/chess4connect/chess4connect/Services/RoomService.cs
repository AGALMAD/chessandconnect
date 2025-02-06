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

        public Room AddToChessRoom(WebSocketHandler player1, WebSocketHandler player2)
        {
            Room room = new Room
            {
                Player1 = player1,
                Player2 = player2,
                StartDate = DateTime.Now,
                Game = Enums.Game.Chess
            };

            rooms.Add(room);

            return room;
        }

        public async Task<Room> AddToConnnectRoomAsync(WebSocketHandler player1, WebSocketHandler player2)
        {
            Room room = new Room
            {
                Player1 = player1,
                Player2 = player2,
                StartDate = DateTime.Now,
                Game = Enums.Game.Connect4
            };

            rooms.Add(room);


            var roomSocketMessage = new SocketMessage<Room>
            {
                Type = Enums.SocketCommunicationType.GAME_START,
                Data = room
            };

            string message = JsonSerializer.Serialize(roomSocketMessage);

            await player1.SendAsync(message);
            await player2.SendAsync(message);

            return room;
        }



        

    }
}
