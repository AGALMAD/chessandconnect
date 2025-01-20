using chess4connect.Enums;

namespace chess4connect.Models.SocketComunication;

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