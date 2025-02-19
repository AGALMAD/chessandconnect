using chess4connect.Models.Games.Chess.Chess.Pieces.Types;
using System.Drawing;

namespace chess4connect.Models.Games.Base
{
    public abstract class BasePiece
    {
        public int Id { get; set; }
        public PieceColor Color { get; set; } 
        public Point Position { get; set; }

        public BasePiece(int id,PieceColor color, Point position)
        {
            Id = id;
            Color = color;
            Position = position;
        }

    }
}
