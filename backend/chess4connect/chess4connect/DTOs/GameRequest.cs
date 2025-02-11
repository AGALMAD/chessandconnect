using chess4connect.Enums;

namespace chess4connect.DTOs
{
    public class GameRequest
    {
        public int OpponentId { get; set; }
        public GameType Game { get; set; }
    }
}
