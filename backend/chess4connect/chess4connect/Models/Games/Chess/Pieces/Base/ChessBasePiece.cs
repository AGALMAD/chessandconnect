using chess4connect.Models.Games.Base;
using chess4connect.Models.Games.Chess.Pieces.Types;
using System.Drawing;

namespace chess4connect.Models.Games.Chess.Pieces.Base;

public abstract class ChessBasePiece: BasePiece
{
    public new int Id { get; set; }
    public new ChessPieceColor Color { get; set; }
    public new Point Position { get; set; }
    public PieceType PieceType { get; set; }
    public List<Point> BasicMovements { get; set; }

    public ChessBasePiece(int id, PieceType pieceType, ChessPieceColor color, Point position): base(id, color, position)
    {
        PieceType = pieceType;
        Id = id;
        Color = color;
        Position = position;

        GetBasicMovements();
    }

    protected abstract void GetBasicMovements();
    

}
