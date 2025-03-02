using chess4connect.Enums;

namespace chess4connect.DTOs
{
    public class GameHistoryDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int OpponentId { get; set; }
        public int Duration { get; set; }
        public GameResult PlayState { get; set; }
        public GameType Game { get; set; }
    }
}
