using chess4connect.Enums;

namespace chess4connect.Models;

public class Play
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int opponentId { get; set; }
    public DateTime startDate { get; set; }
    public DateTime endDate { get; set; }
    public PlayState PlayState { get; set; }
    public int gameId { get; set; }


}
