using chess4connect.Enums;

namespace chess4connect.Models.SocketComunication;

public class SocketMessage
{
    public SocketComunicationType Type { get; set; }
}

public class SocketMessage<T> : SocketMessage
{
    public T Data { get; set; }
}
