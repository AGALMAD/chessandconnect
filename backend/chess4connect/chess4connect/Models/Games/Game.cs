using chess4connect.Enums;
using chess4connect.Models.Games.Base;

namespace chess4connect.Models.Games;

public class Game
{
    public GameType GameType { get; set; }
    public BaseBoard Board { get; set; }
    public DateTime StartDate { get; set; }

}
