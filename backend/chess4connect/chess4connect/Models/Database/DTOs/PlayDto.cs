using chess4connect.Enums;
using chess4connect.Models.Database.Entities.Games;

namespace chess4connect.Models.Database.DTOs;

public class PlayDto
{
    public int Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public GameResult PlayState { get; set; }
    public Game Game { get; set; }
}
