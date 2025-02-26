using chess4connect.Models.Games.Base;
using chess4connect.Models.Games.Chess.Chess.Pieces.Types;
using System.Drawing;

namespace chess4connect.Models.Games.Chess.Chess.Pieces.Base;

public abstract class ChessBasePiece
{
    public int Id { get; set; }
    public bool Player1Piece { get; set; }
    public Point Position { get; set; }
    public PieceType PieceType { get; set; }
    public List<Point> BasicMovements { get; set; }
    public bool HasMoved { get; set; } = false;

    public ChessBasePiece(int id, PieceType pieceType, bool pieceColor, Point position)
    {
        PieceType = pieceType;
        Id = id;
        Player1Piece = pieceColor;
        Position = position;

        GetBasicMovements();
    }

    protected abstract void GetBasicMovements();
    

}
