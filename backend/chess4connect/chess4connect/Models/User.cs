using chess4connect.Enums;

namespace chess4connect.Models;

public class User
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
    public string AvatarImageUrl { get; set; }
    public bool Banned { get; set; }


}
