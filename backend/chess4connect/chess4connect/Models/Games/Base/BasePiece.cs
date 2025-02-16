using chess4connect.Models.Games.Chess.Chess.Pieces.Types;
using System.Drawing;

namespace chess4connect.Models.Games.Base
{
    public abstract class BasePiece
    {
        public int Id { get; set; }
        public ChessPieceColor Color { get; set; } 
        public Point Position { get; set; }

        public BasePiece(int id,ChessPieceColor color, Point position)
        {
            Id = id;
            Color = color;
            Position = position;
        }

    }
}
