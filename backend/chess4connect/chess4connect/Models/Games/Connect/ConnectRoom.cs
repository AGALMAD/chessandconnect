using chess4connect.Enums;
using chess4connect.Models.Games.Base;
using chess4connect.Models.Games.Chess;
using chess4connect.Models.Games.Chess.Chess.Pieces.Types;
using chess4connect.Models.SocketComunication.Handlers;
using chess4connect.Models.SocketComunication.MessageTypes;
using System.Text.Json;

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


    public async Task DropConnectPiece(ConnectDropPieceRequest dropPieceRequest)
    {
        int response = Game.Board.DropPiece(dropPieceRequest.Column);

        if (response == 0)
        {
            await SendBoard();

        }

        if (response == 1)
        {
            await SendWinMessage();

        }


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

    public override async Task SendWinMessage()
    {
        int winnerId = Game.Board.Turn == PieceColor.WHITE ? Player1Handler.Id : Player2Handler.Id;


        //Mensaje con el id del ganador
        var winnerMessage = new SocketMessage<int>
        {
            Type = SocketCommunicationType.END_GAME,

            Data = winnerId,
        };

        string stringWinnerMessage = JsonSerializer.Serialize(winnerMessage);

        await SendMessage(stringWinnerMessage);
    }

}
