using chess4connect.Enums;
using chess4connect.Models.Games;
using System.ComponentModel.DataAnnotations.Schema;

namespace chess4connect.Models.Database.Entities;

public class Play
{
    public int Id { get; set; }
    public int WinnerId { get; set; }
    public GameResult GameResult { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public GameType Game { get; set; }
    public List<PlayDetail> PlayDetails { get; set; } = new List<PlayDetail>();
}
