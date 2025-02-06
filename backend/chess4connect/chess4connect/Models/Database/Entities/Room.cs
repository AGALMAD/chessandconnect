using chess4connect.Enums;
using chess4connect.Models.SocketComunication.Handlers;

namespace chess4connect.Models.Database.Entities
{
    public class Room
    {
        public int Id { get; set; }
        public WebSocketHandler Player1 { get; set; }
        public WebSocketHandler Player2 { get; set; }
        public DateTime StartDate { get; set; }
        public Game Game { get; set; }
    }
}
