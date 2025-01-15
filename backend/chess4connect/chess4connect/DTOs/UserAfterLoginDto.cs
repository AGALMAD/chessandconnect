using chess4connect.Models;

namespace chess4connect.DTOs;

public class UserAfterLoginDto
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
    public string AvatarImageUrl { get; set; }
    public bool Banned { get; set; }

    public List<Play> Plays { get; set; } = new List<Play>();
}
