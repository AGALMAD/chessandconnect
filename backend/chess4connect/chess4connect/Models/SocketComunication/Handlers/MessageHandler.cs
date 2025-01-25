using chess4connect.Enums;
using chess4connect.Models;
using chess4connect.Models.SocketComunication.MessageTypes;
using System.Text.Json;

namespace chess4connect.Models.SocketComunication.Handlers;

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

        }

        return "";
    }
}
