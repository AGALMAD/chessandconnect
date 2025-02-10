using chess4connect.Models.Games.Base;

namespace chess4connect.Models.Games;

public class Game
{
    public Room Room { get; set; }
    public BaseBoard Board { get; set; }
    public DateTime StartDate { get; set; }

}
