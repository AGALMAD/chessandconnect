using chess4connect.Enums;

namespace chess4connect.Models.Database.DTOs
{
    public class GameRequest
    {
        public int OpponentId { get; set; }
        public Game Game { get; set; }
    }
}
