using chess4connect.Enums;

namespace chess4connect.Models.SocketComunication.MessageTypes;

public class SocketMessage
{
    public virtual SocketCommunicationType Type { get; set; }
}

public class SocketMessage<T> : SocketMessage
{
    public T Data { get; set; }
}


public class GameSocketMessage<T> : SocketMessage
{
    public override SocketCommunicationType Type => SocketCommunicationType.GAME;
    public T Data { get; set; }
}
public class ConnectionSocketMessage<T> : SocketMessage
{
    public override SocketCommunicationType Type => SocketCommunicationType.CONNECTION;
    public T Data { get; set; }
}

public class ChatSocketMessage<T> : SocketMessage
{
    public override SocketCommunicationType Type => SocketCommunicationType.CHAT;
    public T Data { get; set; }
}

