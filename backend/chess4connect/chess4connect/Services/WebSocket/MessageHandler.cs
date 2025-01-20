using chess4connect.Enums;
using chess4connect.Models;
using chess4connect.Models.SocketComunication;
using System.Text.Json;

namespace chess4connect.Services.WebSocket;

public class MessageHandler
{

    public async Task<string> ManageMessage(string message)
    {
        //Paso a Json
        var messageJson = JsonSerializer.Deserialize<SocketMessage>(message);

        //Obtiene el tipo 
        SocketComunicationType type = messageJson.Type;

        //Lo gestiona su respectivo servicio
        switch (type)
        {
            case SocketComunicationType.GAME:

                break;

            case SocketComunicationType.CHAT:
                break;

            case SocketComunicationType.FRIEND:
                break;
        }

    }
}
