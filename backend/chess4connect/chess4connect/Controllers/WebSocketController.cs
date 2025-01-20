using chess4connect.Models.Database.Entities;
using chess4connect.Services;
using chess4connect.Services.WebSocket;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace chess4connect.Controllers;

[Route("socket")]
[ApiController]
public class WebSocketController : ControllerBase
{
    MessageHandler _socketService;
    UserService _userService;

    public WebSocketController(MessageHandler socketService, UserService userService) { 
        _socketService = socketService;
        _userService = userService;
    }

    [Authorize]
    [HttpGet]
    public async Task ConnectAsync()
    {
        //Si no es una usuario autorizado termina la ejecución
        User user = await GetAuthorizedUser();

        if (user == null)
            HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;


        //Comunica a todos los amigos de la conexión



        // Si la petición es de tipo websocket la aceptamos
        if (HttpContext.WebSockets.IsWebSocketRequest)
        {
            // Aceptamos la solicitud
            WebSocket webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();

            // Manejamos la solicitud.
            await HandleWebsocketAsync(webSocket);
        }
        // En caso contrario la rechazamos
        else
        {
            HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        }

        

    }//Cierre de conexión

    private async Task HandleWebsocketAsync(WebSocket webSocket)
    {
        // Mientras que el websocket del cliente esté conectado
        while (webSocket.State == WebSocketState.Open)
        {
            // Leemos el mensaje
            string message = await ReadAsync(webSocket);

            if (!string.IsNullOrWhiteSpace(message))
            {
                //El servicio gestiona el mensage
                string outMessage = _socketService.ManageMessage(message);




                // Enviamos respuesta al cliente
                await SendAsync(webSocket, outMessage);
            }

            Console.WriteLine("Conectado");
        }
    }

    private async Task<string> ReadAsync(WebSocket webSocket, CancellationToken cancellation = default)
    {
        // Creo un buffer para almacenar temporalmente los bytes del contenido del mensaje
        byte[] buffer = new byte[4096];
        // Creo un StringBuilder para poder ir creando poco a poco el mensaje en formato texto
        StringBuilder stringBuilder = new StringBuilder();
        // Creo un booleano para saber cuándo termino de leer el mensaje
        bool endOfMessage = false;

        do
        {
            // Recibo el mensaje pasándole el buffer como parámetro
            WebSocketReceiveResult result = await webSocket.ReceiveAsync(buffer, cancellation);

            // Si el resultado que se ha recibido es de tipo texto lo decodifico y lo meto en el StringBuilder
            if (result.MessageType == WebSocketMessageType.Text)
            {
                // Decodifico el contenido recibido
                string message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                // Lo añado al StringBuilder
                stringBuilder.Append(message);
            }
            // Si el resultado que se ha recibido entonces cerramos la conexión
            else if (result.CloseStatus.HasValue)
            {
                // Cerramos la conexión
                await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, cancellation);
            }

            // Guardamos en nuestro booleano si hemos recibido el final del mensaje
            endOfMessage = result.EndOfMessage;
        }
        // Repetiremos iteración si el socket permanece abierto y no se ha recibido todavía el final del mensaje
        while (webSocket.State == WebSocketState.Open && !endOfMessage);

        // Finalmente devolvemos el contenido del StringBuilder
        return stringBuilder.ToString();
    }

    private Task SendAsync(WebSocket webSocket, string message, CancellationToken cancellation = default)
    {
        // Codificamos a bytes el contenido del mensaje
        byte[] bytes = Encoding.UTF8.GetBytes(message);

        // Enviamos los bytes al cliente marcando que el mensaje es un texto
        return webSocket.SendAsync(bytes, WebSocketMessageType.Text, true, cancellation);
    }


    private Task NotifyConnectionAllFriends()
    {


        


        return Task.CompletedTask;
    }


    private async Task<User> GetAuthorizedUser()
    {
        // Pilla el usuario autenticado según ASP
        System.Security.Claims.ClaimsPrincipal currentUser = this.User;
        string idString = currentUser.Claims.First().ToString().Substring(3); // 3 porque en las propiedades sale "id: X", y la X sale en la tercera posición

        // Pilla el usuario de la base de datos
        return await _userService.GetUserByStringId(idString);
    }


}
