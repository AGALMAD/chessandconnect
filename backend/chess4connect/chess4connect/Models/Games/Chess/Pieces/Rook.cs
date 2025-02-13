using chess4connect.Models.Games.Base;
using chess4connect.Models.Games.Chess.Pieces.Types;
using System.Drawing;

namespace chess4connect.Models.Games.Chess.Pieces
{
    public class Rook : BasePiece
    {
        public Rook(int id, Types.Color color, Point position) : base(id, PieceType.ROOK, color, position) { }

        protected List<Point> BasicMovements()
        {
            List<Point> basicMovements = new List<Point>();

            for (int i = 1; i < 8; i++)
            {
                basicMovements.Add(new Point(i, 0));
                basicMovements.Add(new Point(0, i));
                basicMovements.Add(new Point(-i, 0));
                basicMovements.Add(new Point(0, -i));
            }
            return basicMovements;
        }
    }
}
