using chess4connect.Models.Games.Base;
using chess4connect.Models.Games.Chess.Pieces.Base;
using chess4connect.Models.Games.Chess.Pieces.Types;
using System.Drawing;

namespace chess4connect.Models.Games.Chess.Pieces
{
    public class King : ChessBasePiece
    {
        public King(int id, ChessPieceColor color, Point position) : base(id, PieceType.KING, color, position) { }

        protected List<Point> BasicMovements()
        {
            return new List<Point>
            {
                new Point(1,0), new Point(0,1), new Point(-1, 0), new Point(0, -1),
                new Point(1, 1), new Point(1,-1), new Point(-1, 1), new Point(-1, -1)
            };
        }
    }
}
