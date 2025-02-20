using chess4connect.Models.Games.Base;
using chess4connect.Models.Games.Chess.Chess.Pieces.Base;
using chess4connect.Models.Games.Chess.Chess.Pieces.Types;
using System.Drawing;

namespace chess4connect.Models.Games.Chess.Chess.Pieces
{
    public class Queen : ChessBasePiece
    {
        public Queen(int id, PieceColor color, Point position) : base(id, PieceType.QUEEN, color, position) { }


        protected override void GetBasicMovements()
        {
            BasicMovements = new List<Point>
            {
                //horizontal and vertical
                new Point(1, 0),
                new Point(0, 1),
                new Point(-1, 0),
                new Point(0, -1),

                //diagonal
                new Point(1, 1),
                new Point(1, -1),
                new Point(-1, 1),
                new Point(-1, -1)
            };
            
        }
    }
}
