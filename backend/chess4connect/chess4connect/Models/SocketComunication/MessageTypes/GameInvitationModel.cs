using chess4connect.Enums;

namespace chess4connect.Models.SocketComunication.MessageTypes;

public class GameInvitationModel
{
    public int UserId { get; set; }
    public int FriendId { get; set; }
    public FriendshipState State { get; set; }
}
