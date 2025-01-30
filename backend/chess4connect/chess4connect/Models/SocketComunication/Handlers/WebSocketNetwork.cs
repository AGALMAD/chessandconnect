using chess4connect.Enums;
using chess4connect.Models.Database.Entities;
using chess4connect.Models.SocketComunication.Handlers.Services;
using chess4connect.Models.SocketComunication.MessageTypes;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Extensions;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace chess4connect.Models.SocketComunication.Handlers;

public class WebSocketNetwork
{
    private FriendRequestService _friendRequestService;
    //Diccionario de conexiones, almacena el id del usuario y el websocket de su conexión
    private ConcurrentDictionary<int, WebSocketHandler> _handlers = new ConcurrentDictionary<int, WebSocketHandler>();

    private ConcurrentDictionary<int, Queue<string>> _pendingUserMessages = new ConcurrentDictionary<int, Queue<string>>();

    private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

    private WebSocketNetwork(FriendRequestService friendRequestService) 
    { 
        _friendRequestService = friendRequestService;
    }

    public async Task HandleAsync(User user, WebSocket webSocket)
    {
        // Creamos un nuevo WebSocketHandler a partir del WebSocket recibido y lo añadimos a la lista
        WebSocketHandler handler = await AddWebsocketAsync(user, webSocket);

        // Esperamos a que el WebSocketHandler termine de manejar la conexión
        await handler.HandleAsync();

    }

    private async Task<WebSocketHandler> AddWebsocketAsync(User user, WebSocket webSocket)
    {

        // Esperamos a que haya un hueco disponible
        await _semaphore.WaitAsync();

        // Sección crítica
        // Creamos un nuevo WebSocketHandler, nos suscribimos a sus eventos y lo añadimos a la lista
        WebSocketHandler newHandler = new WebSocketHandler(user.Id, webSocket);
        newHandler.Disconnected += OnDisconnectedAsync;
        newHandler.MessageReceived += OnMessageReceivedAsync;
        _handlers.TryAdd(user.Id, newHandler);

        // Liberamos el semáforo
        _semaphore.Release();

        //Copia de todos los handlers
        WebSocketHandler[] allHandlers = _handlers.Values.ToArray();

        var connectionMessage = new ConnectionSocketMessage<ConnectionModel>
        {
            Data = new ConnectionModel
            {
                Type = ConnectionType.Connected,
                UserId = newHandler.Id,
                UsersCounter = allHandlers.Length,

            }
        };

        string stringConnectionMessage = JsonSerializer.Serialize(connectionMessage);

        //Mensaje de conexión a todos los usuarios conectados
        await Broadcast(allHandlers, newHandler, stringConnectionMessage);

        Console.WriteLine("Usuario conectado");

        // Verifica si hay mensajes pendientes y envíalos
        if (_pendingUserMessages.TryRemove(user.Id, out var pendingMessages))
        {
            while (pendingMessages.Count > 0)
            {
                string pendingMessage = pendingMessages.Dequeue();
                await newHandler.SendAsync(pendingMessage);
            }
        }

        return newHandler;

    }




    private async Task OnDisconnectedAsync(WebSocketHandler disconnectedHandler)
    {
        // Esperamos a que haya un hueco disponible
        await _semaphore.WaitAsync();

        // Sección crítica
        // Nos desuscribimos de los eventos y eliminamos el WebSocketHandler de la lista
        disconnectedHandler.Disconnected -= OnDisconnectedAsync;
        disconnectedHandler.MessageReceived -= OnMessageReceivedAsync;
        _handlers.TryRemove(disconnectedHandler.Id, out disconnectedHandler);

        // Liberamos el semáforo
        _semaphore.Release();


        //Copia de todos los handlers
        WebSocketHandler[] allHandlers = _handlers.Values.ToArray();

        var disconnectionMessage = new ConnectionSocketMessage<ConnectionModel>
        {
            Data = new ConnectionModel
            {
                Type = ConnectionType.Disconnected,
                UserId = disconnectedHandler.Id,
                UsersCounter = allHandlers.Length,
                
            }
        };

        string stringDisconnectionMessage = JsonSerializer.Serialize(disconnectionMessage);

        //Mensaje de desconexión a todos los usuarios conectados
        await Broadcast(allHandlers, disconnectedHandler, stringDisconnectionMessage);

        Console.WriteLine("Usuario desconectado");

    }

    private async Task Broadcast(WebSocketHandler[] allHandlers, WebSocketHandler webSocketHandler, string stringMessage)
    {
        // Lista donde guardar las tareas de envío de mensajes
        List<Task> tasks = new List<Task>();

        // Enviamos el mensaje al resto de usuarios
        foreach (WebSocketHandler handler in allHandlers)
        {
            tasks.Add(handler.SendAsync(stringMessage));
        }

        // Esperamos a que todas las tareas de envío de mensajes se completen
        await Task.WhenAll(tasks);
    }

    private Task OnMessageReceivedAsync(string message)
    {
        // Lista donde guardar las tareas de envío de mensajes
        List<Task> tasks = new List<Task>();
        // Guardamos una copia de los WebSocketHandler para evitar problemas de concurrencia
        WebSocketHandler[] handlers = _handlers.Values.ToArray();


        // Enviamos un mensaje personalizado al nuevo usuario y otro al resto
        SocketMessage recived = JsonSerializer.Deserialize<SocketMessage>(message);
        switch (recived.Type)
        {
            case SocketCommunicationType.GAME:

                break;

            case SocketCommunicationType.CHAT:

                break;

            case SocketCommunicationType.CONNECTION:

                break;

            case SocketCommunicationType.FRIEND:

                //var request = JsonSerializer.Deserialize<FriendshipRequestModel>(message);

                //var friendship = _friendRequestService.requestFriendship(request.UserId, request.FriendId);

                //WebSocketHandler address = _handlers.GetValueOrDefault(request.UserId);

                //string stringMessage = JsonSerializer.Serialize(friendship);

                //tasks.Add(address.SendAsync(stringMessage));

                                
                break;

        }
        return Task.CompletedTask;

    }

    public WebSocketHandler GetSocketByUserId(int id)
    {
        return _handlers.FirstOrDefault(p => p.Key == id).Value;
    }

    public void StorePendingMessage(int userId, string message)
    {
        // Obtenemos la cola del usuario o creamos una nueva si no existe
        var queue = _pendingUserMessages.GetOrAdd(userId, _ => new Queue<string>());

        // Agregamos el mensaje a la cola
        queue.Enqueue(message);
    }


}
