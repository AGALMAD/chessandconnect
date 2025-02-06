using chess4connect.Models.Database.Entities;
using chess4connect.Models.SocketComunication.Handlers;

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

        public Room addToChessRoom(WebSocketHandler player1, WebSocketHandler player2)
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

        public Room addToConnnectRoom(WebSocketHandler player1, WebSocketHandler player2)
        {
            Room room = new Room
            {
                Player1 = player1,
                Player2 = player2,
                StartDate = DateTime.Now,
                Game = Enums.Game.Connect4
            };

            rooms.Add(room);

            return room;
        }

        

    }
}
