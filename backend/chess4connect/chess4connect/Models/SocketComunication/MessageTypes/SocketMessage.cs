using chess4connect.Enums;

namespace chess4connect.Models.SocketComunication.MessageTypes;

public class SocketMessage
{
    public virtual SocketComunicationType Type { get; set; }
}

public class SocketMessage<T> : SocketMessage
{
    public T Data { get; set; }
}


public class GameSocketMessage<T> : SocketMessage
{
    public override SocketComunicationType Type => SocketComunicationType.GAME;
    public T Data { get; set; }
}
public class ConnectionSocketMessage<T> : SocketMessage
{
    public override SocketComunicationType Type => SocketComunicationType.FRIEND;
    public T Data { get; set; }
}

public class ChatSocketMessage<T> : SocketMessage
{
    public override SocketComunicationType Type => SocketComunicationType.CHAT;
    public T Data { get; set; }
}

