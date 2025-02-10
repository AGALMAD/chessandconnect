using chess4connect.Models.Database.Entities.Games.Base;

namespace chess4connect.Models.Database.Entities.Games;

public class Game
{
    public Room Room { get; set; }
    public BaseBoard Board { get; set; }
    public DateTime StartDate { get; set; }

}
