using chess4connect.Enums;

namespace chess4connect.Models.SocketComunication.MessageTypes;

public class FriendConnectionModel
{
    public FriendComunicationType Type => FriendComunicationType.FriendConnected;
    public int FriendId { get; set; }
}

public class FriendDisconnectedModel
{
    public FriendComunicationType Type => FriendComunicationType.FriendDisconnected;
    public int FriendId { get; set; }
}


public class FriendshipInvitationModel
{
    public FriendComunicationType Type => FriendComunicationType.FriendshipInvitation;
    public int UserId { get; set; }
}

public class UserConnectionModel
{
    public FriendComunicationType Type => FriendComunicationType.UserConnected;
    public int UserCounter { get; set; }
}

public class UserDisconnectedModel
{
    public FriendComunicationType Type => FriendComunicationType.UserDisconnected;
    public int UserCounter { get; set; }
}



