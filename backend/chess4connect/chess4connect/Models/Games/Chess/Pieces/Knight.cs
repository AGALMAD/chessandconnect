using chess4connect.Models.Games.Base;
using chess4connect.Models.Games.Chess.Pieces.Base;
using chess4connect.Models.Games.Chess.Pieces.Types;
using System.Drawing;

namespace chess4connect.Models.Games.Chess.Pieces
{
    public class Knight : ChessBasePiece
    {
        public Knight(int id, ChessPieceColor color, Point position) : base(id, PieceType.KNIGHT, color, position) { }

        protected override void GetBasicMovements()
        {
            BasicMovements = new List<Point>()
            {
            new Point(2, 1),  new Point(2, -1), new Point(-2, 1), new Point(-2, -1),
            new Point(1, 2),  new Point(1, -2), new Point(-1, 2), new Point(-1, -2)
            };
        }

    }
}
