using chess4connect.Enums;
using chess4connect.Models.Games.Base;
using chess4connect.Models.Games.Chess.Chess;

namespace chess4connect.Models.Games.Chess;

public class ChessGame: BaseGame
{
    public ChessBoard Board { get; set; }

    public ChessGame(GameType gameType,DateTime startDateTime, ChessBoard board): base(gameType, startDateTime)
    {
        Board = board;
    }
}
