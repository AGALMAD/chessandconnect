using chess4connect.Enums;

namespace chess4connect.Models.SocketComunication;

public class SocketMessage<T>
{
    public SocketComunicationType Type { get; set; }

    public T Data { get; set; }
}
