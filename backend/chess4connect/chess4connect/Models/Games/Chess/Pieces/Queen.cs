using chess4connect.Models.Games.Base;
using System.Drawing;

namespace chess4connect.Models.Games.Chess.Pieces
{
    public class Queen : BasePiece
    {
        public Queen(bool host, Point position) : base(host, position) { }

        protected List<Point> BasicMovements()
        {
            List<Point> basicMovements = new List<Point>();

            for (int i = 1; i < 8; i++)
            {
                basicMovements.Add(new Point(i, 0));
                basicMovements.Add(new Point(0, i));
                basicMovements.Add(new Point(-i, 0));
                basicMovements.Add(new Point(0, -i));

                basicMovements.Add(new Point(i, i));
                basicMovements.Add(new Point(i, -i));
                basicMovements.Add(new Point(-i, i));
                basicMovements.Add(new Point(-i, -i));
            }
            return basicMovements;
        }
    }
}
