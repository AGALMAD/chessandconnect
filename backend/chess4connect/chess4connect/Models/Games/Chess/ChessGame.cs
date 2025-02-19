using chess4connect.Enums;
using chess4connect.Models.Games.Base;
using chess4connect.Models.Games.Chess.Chess;
using chess4connect.Models.Games.Chess.Chess.Pieces.Types;

namespace chess4connect.Models.Games.Chess;

public class ChessGame: BaseGame
{
    public ChessBoard Board { get; set; }

    public ChessGame(DateTime startDateTime, ChessBoard board): base(GameType.Chess, startDateTime)
    {
        Board = board;
    }
}
