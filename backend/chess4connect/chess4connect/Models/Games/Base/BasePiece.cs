using System.Drawing;

namespace chess4connect.Models.Games.Base
{
    public class BasePiece
    {
        public int Id { get; set; }
        public bool Host { get; set; }
        public Point Position { get; set; }

        public BasePiece(bool host, Point position)
        {

            Host = host;
            Position = position;
        }

    }
}
