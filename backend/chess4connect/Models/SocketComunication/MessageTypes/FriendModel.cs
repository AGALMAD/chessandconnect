namespace chess4connect.Models.SocketComunication.MessageTypes;

public class FriendModel
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public string AvatarImageUrl { get; set; }
    public bool Connected { get; set; }
}
