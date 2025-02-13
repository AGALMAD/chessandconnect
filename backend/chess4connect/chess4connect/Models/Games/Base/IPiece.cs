using chess4connect.Models.Games.Chess.Pieces.Types;
using System.Drawing;

namespace chess4connect.Models.Games.Base
{
    public interface IPiece
    {
        public int Id { get; set; }
        public ChessPieceColor Color { get; set; } 
        public Point Position { get; set; }

        void Move();
    }
}
