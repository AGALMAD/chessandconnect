using chess4connect.Models.Database.Entities.Games.Base;

namespace chess4connect.Models.Database.Entities;

public class Game
{
    public Room Room { get; set; }
    public BaseBoard Board { get; set; }
}
