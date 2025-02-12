using chess4connect.Models.Games.Base;
using System.Drawing;

namespace chess4connect.Models.Games.Chess.Pieces
{
    public class Bishop : BasePiece
    {
        public Bishop(int id, Chess.Color color, Point position) : base(id, color, position) { }

        protected List<Point> BasicMovements()
        {
            List<Point> basicMovements = new List<Point>();

            for (int i = 1; i < 8; i++) 
            {
                basicMovements.Add(new Point(i, i));
                basicMovements.Add(new Point(i, -i));
                basicMovements.Add(new Point(-i, i));
                basicMovements.Add(new Point(-i, -i));
            }
            return basicMovements;
        }
    }
}
