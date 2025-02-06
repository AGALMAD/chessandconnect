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


        public async Task AddToRoom(Game gamemode, WebSocketHandler player1, WebSocketHandler player2 = null)
        {
            await SendRoomMessage(CreateRoom(gamemode, player1.Id, player2?.Id), player1, player2);
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

        private Room CreateRoom(Game gamemode, int player1Id, int? player2Id)
        {
            Room room = new Room
            {
                Player1Id = player1Id,
                Player2Id = player2Id,
                StartDate = DateTime.Now,
                Game = gamemode
            };
            rooms.Add(room);

            return room;
        }


        

    }
}
