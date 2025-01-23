using chess4connect.Enums;

namespace chess4connect.Models.Database.Entities;

public class Play
{
    public int Id { get; set; }
    public int WinnerId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public List<PlayDetail> PlayDetails { get; set; }

}
