using chess4connect.Enums;
using chess4connect.Models.Games.Base;
using chess4connect.Models.Games.Chess.Chess;

namespace chess4connect.Models.Games.Connect;

public class ConnectGame: BaseGame
{
    public ConnectBoard Board { get; set; }

    public ConnectGame(DateTime startDateTime, ConnectBoard board) : base(GameType.Connect4, startDateTime)
    {
        Board = board;
    }
}
