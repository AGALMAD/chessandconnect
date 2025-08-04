using chess4connect.Enums;

namespace chess4connect.Models.Games
{
    public class RoomRequest
    {
        public int Player1Id { get; set; }
        public int? Player2Id { get; set; }

        public GameType GameType { get; set; }
    }
}
