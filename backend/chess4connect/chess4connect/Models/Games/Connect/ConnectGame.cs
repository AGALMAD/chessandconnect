using chess4connect.Enums;
using chess4connect.Models.Games.Chess.Chess;

namespace chess4connect.Models.Games.Connect;

public class ConnectGame
{
    public ConnectBoard Board { get; set; }

    public ConnectGame(GameType gameType, DateTime startDateTime, ConnectBoard board) : base(gameType, startDateTime)
    {
        Board = board;
    }
}
