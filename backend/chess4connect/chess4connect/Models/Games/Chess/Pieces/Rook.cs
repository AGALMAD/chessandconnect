using chess4connect.Models.Games.Base;
using chess4connect.Models.Games.Chess.Pieces.Base;
using chess4connect.Models.Games.Chess.Pieces.Types;
using System.Drawing;

namespace chess4connect.Models.Games.Chess.Pieces
{
    public class Rook : ChessBasePiece
    {
        public Rook(int id, ChessPieceColor color, Point position) : base(id, PieceType.ROOK, color, position) { }

        protected override void GetBasicMovements()
        {
            BasicMovements = new List<Point>();

            for (int i = 1; i < 8; i++)
            {
                BasicMovements.Add(new Point(i, 0));
                BasicMovements.Add(new Point(0, i));
                BasicMovements.Add(new Point(-i, 0));
                BasicMovements.Add(new Point(0, -i));
            }
        }
    }
}
