using chess4connect.Models.Games.Base;
using chess4connect.Models.Games.Chess;
using chess4connect.Models.SocketComunication.Handlers;

namespace chess4connect.Models.Games.Connect;

public class ConnectRoom: BaseRoom
{
    public ConnectGame Game { get; set; }

    public ConnectRoom(WebSocketHandler player1Handler, WebSocketHandler player2Handler, ConnectGame game): base(player1Handler, player2Handler)
    {
        Player1Handler = player1Handler;
        Player2Handler = player2Handler;

        Game = game;
    }

    public override Task SendBoard()
    {
        throw new NotImplementedException();
    }

    public override Task SendRoom()
    {
        throw new NotImplementedException();
    }

    public override Task MessageHandler(string message)
    {
        throw new NotImplementedException();
    }
}
