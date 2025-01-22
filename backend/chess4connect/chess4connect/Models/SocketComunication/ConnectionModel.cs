using chess4connect.Enums;

namespace chess4connect.Models.SocketComunication;

public class ConnectionModel
{
    public FriendComunicationType Type => FriendComunicationType.FriendConnected;
    public int FriendId { get; set; }
}
