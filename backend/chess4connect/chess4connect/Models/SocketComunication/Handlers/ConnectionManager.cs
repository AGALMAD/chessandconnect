using chess4connect.Models.SocketComunication.Handlers;
using System.Collections.Concurrent;
using System.Net.WebSockets;


public class ConnectionManager
{
    //Diccionario de conexiones, almacena el id del usuario y el websocket de su conexión
    private ConcurrentDictionary<int, WebSocketHandler> _sockets = new ConcurrentDictionary<int, WebSocketHandler>();


    public WebSocketHandler GetSocketByUserId(int id)
    {
        return _sockets.FirstOrDefault(p => p.Key == id).Value;
    }

    public ConcurrentDictionary<int, WebSocketHandler> GetAll()
    {
        return _sockets;
    }

    public int GetUserId(WebSocketHandler socket)
    {
        return _sockets.FirstOrDefault(p => p.Value == socket).Key;
    }

    public void AddSocket(int userId, WebSocketHandler socket)
    {
        _sockets.TryAdd(userId, socket);
    }

    public async Task RemoveSocket(int id)
    {
        WebSocketHandler socket;
        _sockets.TryRemove(id, out socket);

        socket.Dispose();
    }


}
