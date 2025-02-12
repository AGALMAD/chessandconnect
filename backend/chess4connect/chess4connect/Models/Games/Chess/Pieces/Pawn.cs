using chess4connect.Models.Games.Base;
using System.Drawing;

namespace chess4connect.Models.Games.Chess.Pieces
{
    public class Pawn : BasePiece
    {
        public bool FirstMove { get; set; } = true;
        public Pawn(bool host, Point position) : base(host, position) { }

        protected List<Point> BasicMovements()
        {

            int direction = Host ? -1 : 1;

            List<Point> basicMovements = new List<Point>
            {
                new Point(direction, 0)
            };
            if (FirstMove)
            {
                basicMovements.Add(new Point(2 * direction,0));
            }

            return basicMovements;
        }


    }
}

