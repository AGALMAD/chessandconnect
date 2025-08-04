using System.Net.WebSockets;
using System.Text;

namespace chess4connect.Models.SocketComunication.Handlers;

public class WebSocketHandler : IDisposable
{
    private const int BUFFER_SIZE = 4096;

    private readonly WebSocket _webSocket;
    private readonly byte[] _buffer;

    public int Id { get; init; }
    public bool IsOpen => _webSocket.State == WebSocketState.Open;

    // Eventos para notificar cuando se recibe un mensaje o se desconecta un usuario
    public event Func<WebSocketHandler, string, Task> MessageReceived;
    public event Func<WebSocketHandler, Task> Disconnected;

    public WebSocketHandler(int id, WebSocket webSocket)
    {
        Id = id;

        _webSocket = webSocket;
        _buffer = new byte[BUFFER_SIZE];
    }

    public async Task HandleAsync()
    {
        try
        {
            while (IsOpen)
            {
                string message = await ReadAsync();

                if (!string.IsNullOrWhiteSpace(message) && MessageReceived != null)
                {
                    await MessageReceived.Invoke(this, message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error en WebSocket {Id}: {ex.Message}");
        }
        finally
        {
            await CloseSocket();
        }
    }

    private async Task<string> ReadAsync()
    {
        try
        {
            using MemoryStream textStream = new MemoryStream();
            WebSocketReceiveResult receiveResult;

            do
            {
                receiveResult = await _webSocket.ReceiveAsync(_buffer, CancellationToken.None);

                if (receiveResult.MessageType == WebSocketMessageType.Text)
                {
                    textStream.Write(_buffer, 0, receiveResult.Count);
                }
                else if (receiveResult.CloseStatus.HasValue)
                {
                    Console.WriteLine($"Cierre detectado en WebSocket {Id}: {receiveResult.CloseStatus}");
                    await CloseSocket();  
                    return string.Empty;
                }
            }
            while (!receiveResult.EndOfMessage);

            return Encoding.UTF8.GetString(textStream.ToArray());
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error leyendo WebSocket {Id}: {ex.Message}");
            await CloseSocket(); 
            return string.Empty;
        }
    }


    public async Task SendAsync(string message)
    {
        // Si el websocket está abierto, enviamos el mensaje
        if (IsOpen)
        {
            // Convertimos el mensaje a bytes y lo enviamos
            byte[] bytes = Encoding.UTF8.GetBytes(message);
            await _webSocket.SendAsync(bytes, WebSocketMessageType.Text, true, CancellationToken.None);
        }
    }

    public async Task CloseSocket()
    {
        if (_webSocket.State == WebSocketState.Open || _webSocket.State == WebSocketState.Connecting)
        {
            try
            {
                await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Cierre normal", CancellationToken.None);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error cerrando WebSocket {Id}: {ex.Message}");
            }
        }

        Dispose();  // Llama a Dispose para limpiar recursos

        if (Disconnected != null)
        {
            await Disconnected.Invoke(this);  // Notificar la desconexión
        }
    }


    public void Dispose()
    {
        // Cerramos el websocket
        _webSocket.Dispose();
    }


}
