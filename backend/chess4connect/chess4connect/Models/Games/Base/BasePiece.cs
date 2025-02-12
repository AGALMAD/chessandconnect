using System.Drawing;

namespace chess4connect.Models.Games.Base
{
    public class BasePiece
    {
        public int Id { get; set; }
        public Color Color { get; set; } 
        public Point Position { get; set; }

        public BasePiece(Color color, Point position)
        {

            Color = color;
            Position = position;
        }

    }
}
