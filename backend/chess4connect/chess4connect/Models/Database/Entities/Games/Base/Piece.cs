using System.Drawing;

namespace chess4connect.Models.Database.Entities.Games.Base
{
    public class Piece
    {
        public bool Host { get; set; }
        public Point Position { get; set; }

        public Piece(bool host, Point position) {
        
            Host = host;
            Position = position;
        }
       
    }
}
