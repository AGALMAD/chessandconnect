using chess4connect.Models.Games;

namespace chess4connect.DTOs
{
    public class RoomDto
    {
        public int Player1Id { get; set; }
        public int? Player2Id { get; set; }
        public GameDto Game { get; set; }
    }
}
