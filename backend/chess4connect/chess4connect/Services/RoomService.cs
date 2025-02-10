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


        public async Task CreateRoomAsync(Game gamemode, WebSocketHandler player1, WebSocketHandler player2 = null)
        {
            Room room = new Room
            {
                Player1Id = player1.Id,
                Player2Id = player2?.Id,
                StartDate = DateTime.Now,
                Game = gamemode
            };

            rooms.Add(room);

            await SendRoomMessageAsync(room, player1, player2);
        }

        private async Task SendRoomMessageAsync(Room room, WebSocketHandler player1, WebSocketHandler player2)
        {
            var roomSocketMessage = new SocketMessage<Room>
            {
                Type = Enums.SocketCommunicationType.GAME_START,
                Data = room
            };

            string message = JsonSerializer.Serialize(roomSocketMessage);

            await player1.SendAsync(message);
            if(player2 is not null)
            {
                await player2.SendAsync(message);
            }
        }
        

    }
}
