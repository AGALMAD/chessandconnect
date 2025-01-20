using chess4connect.Enums;
using chess4connect.Models.Database.Entities;

namespace chess4connect.Models.Database.DTOs;

public class PlayDto
{
    public int Id { get; set; }
    public User User { get; set; }
    public User opponent { get; set; }
    public DateTime startDate { get; set; }
    public DateTime endDate { get; set; }
    public PlayState PlayState { get; set; }
    public Game Game { get; set; }
}
