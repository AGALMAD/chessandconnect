using chess4connect.Models.Games.Chess.Chess.Pieces.Types;
using System.Drawing;

namespace chess4connect.Models.Games.Chess.Chess.Pieces.Base;

public class ChessPieceWhithOutBasicMovements
{
    public int Id { get; set; }
    public PieceColor Color { get; set; }
    public Point Position { get; set; }
    public PieceType PieceType { get; set; }

}
