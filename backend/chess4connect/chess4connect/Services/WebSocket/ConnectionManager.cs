using System.Collections.Concurrent;
using System.Net.WebSockets;


public class ConnectionManager
{
    //Diccionario de conexiones, almacena el id del usuario y el websocket de su conexión
    private ConcurrentDictionary<int, WebSocket> _sockets = new ConcurrentDictionary<int, WebSocket>();


    public WebSocket GetSocketByUserId(int id)
    {
        return _sockets.FirstOrDefault(p => p.Key == id).Value;
    }

    public ConcurrentDictionary<int, WebSocket> GetAll()
    {
        return _sockets;
    }

    public int GetUserId(WebSocket socket)
    {
        return _sockets.FirstOrDefault(p => p.Value == socket).Key;
    }

    public void AddSocket(int userId, WebSocket socket)
    {
        _sockets.TryAdd(userId, socket);
    }

    public async Task RemoveSocket(int id)
    {
        WebSocket socket;
        _sockets.TryRemove(id, out socket);

        await socket.CloseAsync(closeStatus: WebSocketCloseStatus.NormalClosure,
                                statusDescription: "Closed by the ConnectionManager",
                                cancellationToken: CancellationToken.None);
    }


}
