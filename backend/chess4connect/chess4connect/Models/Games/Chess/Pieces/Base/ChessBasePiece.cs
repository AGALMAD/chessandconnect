using chess4connect.Models.Games.Base;
using chess4connect.Models.Games.Chess.Pieces.Types;
using System.Drawing;

namespace chess4connect.Models.Games.Chess.Pieces.Base;

public class ChessBasePiece : BasePiece
{
    public PieceType PieceType { get; set; }
    public List<Point> PossibleMovements { get; set; } = new List<Point>();

    public ChessBasePiece(int id, PieceType pieceType, ChessPieceColor color, Point position): base(id,color,position)
    {
        PieceType = pieceType;
    }
}
