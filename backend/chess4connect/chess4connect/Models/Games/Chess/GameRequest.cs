using chess4connect.Enums;

namespace chess4connect.Models.Games.Chess.Chess
{
    public class GameRequest
    {
        public int OpponentId { get; set; }
        public GameType Game { get; set; }
    }
}
