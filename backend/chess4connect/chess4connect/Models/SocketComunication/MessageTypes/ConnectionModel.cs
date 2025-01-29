using chess4connect.Enums;

namespace chess4connect.Models.SocketComunication.MessageTypes;

public class ConnectionModel
{
    public ConnectionType Type { get; set; }
    public int UserId { get; set; }
    public int UsersCounter { get; set; }
}

