using chess4connect.DTOs.Games;
using chess4connect.Enums;
using chess4connect.Models.SocketComunication.Handlers;
using chess4connect.Models.SocketComunication.MessageTypes;
using System.Text.Json;

namespace chess4connect.Models.Games.Base;

public abstract class BaseRoom
{
    public WebSocketHandler Player1Handler { get; set; }
    public WebSocketHandler? Player2Handler { get; set; }

    public BaseRoom(WebSocketHandler player1Handler, WebSocketHandler? player2Handler) {
        Player1Handler = player1Handler;
        Player2Handler = player2Handler;
    }
    public abstract Task SendBoard();
    public abstract Task SendWinMessage();
    public abstract Task MessageHandler( string message);


    public async Task SendRoom(GameType gameType)
    {
        int player2Id;
        if (Player2Handler == null)
            player2Id = 0;
        else
            player2Id = Player2Handler.Id;

        var roomMessage = new SocketMessage<RoomDto>
        {
            Type = SocketCommunicationType.GAME_START,

            Data = new RoomDto
            {
                GameType = gameType,
                Player1Id = Player1Handler.Id,
                Player2Id = player2Id,
            }
        };

        string stringRoomMessage = JsonSerializer.Serialize(roomMessage);

        await SendMessage(stringRoomMessage);

    }


    public async Task SendMessage(string message)
    {
        //Envia los movimientos al jugador
        await Player1Handler.SendAsync(message);


        if (Player2Handler != null)
        {
            await Player2Handler.SendAsync(message);
        }
    }



}

