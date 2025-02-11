using chess4connect.Enums;
using chess4connect.Models.Games.Base;

namespace chess4connect.DTOs
{
    public class GameDto
    {

        public GameType GameType { get; set; }
        public List<BasePiece> Pieces { get; set; }
        public DateTime StartDate { get; set; }
    }
}
