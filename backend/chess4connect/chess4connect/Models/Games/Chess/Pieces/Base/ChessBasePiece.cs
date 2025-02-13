using chess4connect.Models.Games.Base;
using chess4connect.Models.Games.Chess.Pieces.Types;
using System.Drawing;

namespace chess4connect.Models.Games.Chess.Pieces.Base;

public class ChessBasePiece : IPiece
{
    public int Id { get; set; }
    public ChessPieceColor Color { get; set; }
    public Point Position { get; set; }
    public PieceType PieceType { get; set; }


    public List<Point> PossibleMovements { get; set; } = new List<Point>();

    public ChessBasePiece(int id, PieceType pieceType, ChessPieceColor color, Point position)
    {
        Id = id;
        Color = color;
        Position = position;

        PieceType = pieceType;
    }

    public void Move()
    {
        throw new NotImplementedException();
    }
}
