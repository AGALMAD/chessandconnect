using chess4connect.Enums;
using Microsoft.EntityFrameworkCore;

namespace chess4connect.Models.Database.Entities;

[Index(nameof(Email), IsUnique = true)]
[Index(nameof(UserName), IsUnique = true)]

public class User
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
    public string AvatarImageUrl { get; set; }
    public bool Banned { get; set; }

    public List<User> Friends { get; set; } = new List<User>();
    public List<Friendship> Requests { get; set; } = new List<Friendship>();
    public List<Play> Plays { get; set; } = new List<Play>();
    public List<User> Friends { get; set; } = new List<User>();


}
