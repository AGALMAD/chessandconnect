using System.Drawing;

namespace chess4connect.Models.Database.Entities
{
    public class Piece
    {
        public string Color { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }

        public Point Position { get; set; }


    }
}
