using chess4connect.Models.Games.Base;
using chess4connect.Models.Games.Chess.Pieces.Base;
using chess4connect.Models.Games.Chess.Pieces.Types;
using System.Drawing;

namespace chess4connect.Models.Games.Chess.Pieces
{
    public class Queen : ChessBasePiece
    {
        public Queen(int id, ChessPieceColor color, Point position) : base(id, PieceType.QUEEN, color, position) { }


        protected override void GetBasicMovements()
        {
            BasicMovements = new List<Point>();

            for (int i = 1; i < 8; i++)
            {
                BasicMovements.Add(new Point(i, 0));
                BasicMovements.Add(new Point(0, i));
                BasicMovements.Add(new Point(-i, 0));
                BasicMovements.Add(new Point(0, -i));

                BasicMovements.Add(new Point(i, i));
                BasicMovements.Add(new Point(i, -i));
                BasicMovements.Add(new Point(-i, i));
                BasicMovements.Add(new Point(-i, -i));
            }
            
        }
    }
}
