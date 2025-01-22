using chess4connect.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace chess4connect.Models.Database.Entities;

public class Play
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public int OpponentId { get; set; }

    public User Opponent { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public PlayState PlayState { get; set; }
    public int GameId { get; set; }
    public Game Game { get; set; }

}
