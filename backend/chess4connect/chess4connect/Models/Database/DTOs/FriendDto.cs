using chess4connect.Models.Database.Entities;

namespace chess4connect.Models.Database.DTOs;

public class FriendDto
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string AvatarImageUrl { get; set; }
    public List<Play> Plays { get; set; } = new List<Play>();
}
