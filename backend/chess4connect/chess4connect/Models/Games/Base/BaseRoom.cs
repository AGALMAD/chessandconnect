using chess4connect.Models.SocketComunication.Handlers;

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
    public abstract Task SendRoom();

    public abstract Task MessageHandler( string message);
    public abstract Task SendWinMessage();

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

