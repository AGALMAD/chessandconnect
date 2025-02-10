using chess4connect.Enums;
using chess4connect.Models.Games;

namespace chess4connect.DTOs;

public class PlayDto
{
    public int Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public GameResult PlayState { get; set; }
    public Game Game { get; set; }
}
