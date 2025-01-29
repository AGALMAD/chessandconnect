using chess4connect.Models.Database.Entities;

namespace chess4connect.Models.Database.DTOs;

public class UserAfterLoginDto
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
    public string AvatarImageUrl { get; set; }
    public bool Banned { get; set; }

    public List<User> Friends { get; set; } = new List<User>();
    public List<Friendship> Requests { get; set; } = new List<Friendship>();
    public List<Play> Plays { get; set; } = new List<Play>();
}
