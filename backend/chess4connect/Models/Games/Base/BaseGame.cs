using chess4connect.Enums;

namespace chess4connect.Models.Games.Base;

public class BaseGame
{
    public GameType GameType { get; set; }
    public DateTime StartDate { get; set; }

    public BaseGame(GameType gameType, DateTime startDate)
    {
        GameType = gameType;
        StartDate = startDate;
    }   
}
