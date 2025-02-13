using chess4connect.Enums;
using chess4connect.Models.Games.Base;

namespace chess4connect.Models.Games;

public class Game<T>
{
    public GameType GameType { get; set; }
    public BaseBoard<T> Board { get; set; }
    public DateTime StartDate { get; set; }

}
