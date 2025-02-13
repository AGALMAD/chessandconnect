using chess4connect.Enums;

namespace chess4connect.Models.Games
{
    public class Room<T>
    {
        public int Player1Id { get; set; }
        public int? Player2Id { get; set; }
        public Game<T> Game { get; set; }
    }
}
