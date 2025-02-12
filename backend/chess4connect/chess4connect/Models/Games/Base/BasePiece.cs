
using chess4connect.Models.Games.Chess;
using System.Drawing;

namespace chess4connect.Models.Games.Base
{
    public class BasePiece
    {
        public int Id { get; set; }
        public Chess.Color Color { get; set; } 
        public Point Position { get; set; }

        public BasePiece(int id, Chess.Color color, Point position)
        {
            Id = id;
            Color = color;
            Position = position;
        }

    }
}
