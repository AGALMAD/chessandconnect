using chess4connect.Enums;

namespace chess4connect.Models.Games
{
    public class Room
    {
        public int Id { get; set; }
        public int Player1Id { get; set; }
        public int? Player2Id { get; set; }
        public GameType Game { get; set; }
    }
}
