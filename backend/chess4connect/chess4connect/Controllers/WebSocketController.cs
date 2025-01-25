using chess4connect.Models.Database.Entities;
using chess4connect.Models.SocketComunication;
using chess4connect.Models.SocketComunication.Handlers;
using chess4connect.Models.SocketComunication.MessageTypes;
using chess4connect.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace chess4connect.Controllers;

[Route("socket")]
[ApiController]
public class WebSocketController : ControllerBase
{
    private readonly WebSocketNetwork _websocketNetwork;
    private readonly UserService _userService;

    public WebSocketController(
        WebSocketNetwork websocketNetwork, 
        UserService userService)
    {
        _websocketNetwork = websocketNetwork;
        _userService = userService;
    }

    [Authorize]
    [HttpGet]
    public async Task ConnectAsync()
    {
        //Si no es una usuario autorizado termina la ejecución
        User user = await GetAuthorizedUser();
        Console.WriteLine(user.ToString());
        if (user == null)
            HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;


        // Si la petición es de tipo websocket la aceptamos
        if (HttpContext.WebSockets.IsWebSocketRequest)
        {
            // Aceptamos la solicitud
            WebSocket webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();

            // Manejamos la solicitud.
            await _websocketNetwork.HandleAsync(user,webSocket);
        }
        // En caso contrario la rechazamos
        else
        {
            HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        }


    }//Cierre de conexión

  
    private async Task<User> GetAuthorizedUser()
    {
        // Pilla el usuario autenticado según ASP
        System.Security.Claims.ClaimsPrincipal currentUser = this.User;
        string idString = currentUser.Claims.First().ToString().Substring(3); // 3 porque en las propiedades sale "id: X", y la X sale en la tercera posición

        // Pilla el usuario de la base de datos
        return await _userService.GetUserByStringId(idString);
    }

}
