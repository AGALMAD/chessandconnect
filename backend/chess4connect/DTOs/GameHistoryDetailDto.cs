using chess4connect.Enums;
using chess4connect.Models.Database.Entities;

namespace chess4connect.DTOs
{
    public class GameHistoryDetailDto
    {
        public int Id { get; set; }
        public User User { get; set; }
        public User Opponent { get; set; }
        public int Duration { get; set; }
        public GameResult PlayState { get; set; }
        public GameType Game { get; set; }
    }
}
