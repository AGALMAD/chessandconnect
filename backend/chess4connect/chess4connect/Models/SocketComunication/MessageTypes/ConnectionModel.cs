using chess4connect.Enums;

namespace chess4connect.Models.SocketComunication.MessageTypes;

public class ConnectionModel
{
    public ConnectionType Type;
    public int UserId { get; set; }
    public int UsersCounter { get; set; }
}

