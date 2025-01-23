using chess4connect.Enums;
using chess4connect.Models.Database.Entities;
using chess4connect.Models.SocketComunication.MessageTypes;
using Microsoft.AspNetCore.DataProtection;
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
    //Diccionario de conexiones, almacena el id del usuario y el websocket de su conexión
    private ConcurrentDictionary<int, WebSocketHandler> _handlers = new ConcurrentDictionary<int, WebSocketHandler>();

    private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);


    public async Task HandleAsync(User user, WebSocket webSocket)
    {
        // Creamos un nuevo WebSocketHandler a partir del WebSocket recibido y lo añadimos a la lista
        WebSocketHandler handler = await AddWebsocketAsync(user, webSocket);

        // Notificamos a los usuarios que un nuevo usuario se ha conectado
        await NotifyUserConnectedAsync(handler);
        // Esperamos a que el WebSocketHandler termine de manejar la conexión
        await handler.HandleAsync();
    }

    private async Task<WebSocketHandler> AddWebsocketAsync(User user, WebSocket webSocket)
    {

        // Esperamos a que haya un hueco disponible
        await _semaphore.WaitAsync();

        // Sección crítica
        // Creamos un nuevo WebSocketHandler, nos suscribimos a sus eventos y lo añadimos a la lista
        WebSocketHandler handler = new WebSocketHandler(user.Id, webSocket);
        handler.Disconnected += OnDisconnectedAsync;
        handler.MessageReceived += OnMessageReceivedAsync;
        _handlers.TryAdd(user.Id, handler);

        return handler;

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

        // Lista donde guardar las tareas de envío de mensajes
        List<Task> tasks = new List<Task>();
        // Guardamos una copia de los WebSocketHandler para evitar problemas de concurrencia
        WebSocketHandler[] handlers = _handlers.Values.ToArray();

        var message = new ConnectionSocketMessage<ConnectionModel>
        {
            Data = new ConnectionModel
            {
                Type = ConnectionType.Disconnected,
                UserId = disconnectedHandler.Id,
                UsersCounter = handlers.Length,
                
            }
        };

        //Paso a string
        string stringMessage = JsonSerializer.Serialize(message);

        // Enviamos el mensaje al resto de usuarios
        foreach (WebSocketHandler handler in handlers)
        {
            tasks.Add(handler.SendAsync(stringMessage));
        }

        // Esperamos a que todas las tareas de envío de mensajes se completen
        await Task.WhenAll(tasks);
    }

    private Task OnMessageReceivedAsync(WebSocketHandler userHandler, string message)
    {
        // Lista donde guardar las tareas de envío de mensajes
        List<Task> tasks = new List<Task>();
        // Guardamos una copia de los WebSocketHandler para evitar problemas de concurrencia
        WebSocketHandler[] handlers = _handlers.Values.ToArray();

        string messageToMe = $"Tú: {message}";
        string messageToOthers = $"Usuario {userHandler.Id}: {message}";

        // Enviamos un mensaje personalizado al nuevo usuario y otro al resto
        foreach (WebSocketHandler handler in handlers)
        {
            string messageToSend = handler.Id == userHandler.Id ? messageToMe : messageToOthers;
            tasks.Add(handler.SendAsync(messageToSend));
        }

        // Devolvemos una tarea que se completará cuando todas las tareas de envío de mensajes se completen
        return Task.WhenAll(tasks);
    }

    public WebSocketHandler GetSocketByUserId(int id)
    {
        return _handlers.FirstOrDefault(p => p.Key == id).Value;
    }

    private async Task NotifyConnectionToAllFriends(User user, ConnectionType connectionType)
    {

        //Comunica a todos los usuarios conectados de nuestra conexión
        foreach (var friend in user.Friends)
        {
            //Obtenemos el socket de nuestro amigo
            WebSocketHandler handler = GetSocketByUserId(friend.Id);

            if (handler != null)
            {
                var message = new ConnectionSocketMessage<FriendConnectionModel>
                {
                    Data = new FriendConnectionModel
                    {
                        Type = connectionType,
                        FriendId = friend.Id,
                    }
                };
                //Paso a string
                string stringMessage = JsonSerializer.Serialize(message);

                await handler.SendAsync(stringMessage);
            }
        }

    }

    private Task NotifyConnectionToAllUsers(WebSocketHandler handler, ConnectionType connectionType)
    {
        // Lista donde guardar las tareas de envío de mensajes
        List<Task> tasks = new List<Task>();
        // Guardamos una copia de los WebSocketHandler para evitar problemas de concurrencia
        WebSocketHandler[] handlers = _handlers.Values.ToArray();


        //Comunica a todos los usuarios conectados de nuestra conexión
        foreach (WebSocketHandler webSocketHandler in handlers)
        {

            var message = new ConnectionSocketMessage<UserConnectionModel>
            {
                Data = new UserConnectionModel
                {
                    Type = connectionType,
                    UserCounter = handlers.Length ,
                }
            };

            //Paso a string
            string stringMessage = JsonSerializer.Serialize(message);

            tasks.Add(handler.SendAsync(stringMessage));


        }


        return Task.WhenAll(tasks);
    }

}
