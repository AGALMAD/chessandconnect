using chess4connect.Models.Games.Chess.Pieces.Types;
using System.Drawing;

namespace chess4connect.Models.Games.Base
{
    public class BasePiece
    {
        public int Id { get; set; }
        public PieceType PieceType { get; set; }
        public Chess.Pieces.Types.Color Color { get; set; } 
        public Point Position { get; set; }

        public BasePiece(int id,PieceType pieceType,Chess.Pieces.Types.Color color, Point position)
        {
            Id = id;
            PieceType = pieceType;
            Color = color;
            Position = position;
        }

    }
}
