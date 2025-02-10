using System.Drawing;

namespace chess4connect.Models.Database.Entities.Games.Base
{
    public class BasePiece
    {
        public bool Host { get; set; }
        public Point Position { get; set; }

        public BasePiece(bool host, Point position) {
        
            Host = host;
            Position = position;
        }
       
    }
}
