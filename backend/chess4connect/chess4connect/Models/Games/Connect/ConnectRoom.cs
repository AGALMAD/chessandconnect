using chess4connect.Models.Games.Base;
using chess4connect.Models.Games.Chess;

namespace chess4connect.Models.Games.Connect;

public class ConnectRoom: BaseRoom
{
    public ConnectGame Game { get; set; }

    public ConnectRoom(int player1Id, int player2Id, ConnectGame game): base(player1Id, player2Id)
    {

        Game = game;
    }
}
