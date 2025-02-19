using chess4connect.Models.Games.Base;
using chess4connect.Models.Games.Chess.Chess.Pieces.Base;
using chess4connect.Models.Games.Chess.Chess.Pieces.Types;
using System.Drawing;

namespace chess4connect.Models.Games.Chess.Chess.Pieces
{
    public class Bishop : ChessBasePiece
    {
        public Bishop(int id, PieceColor color, Point position) : base(id, PieceType.BISHOP, color, position) 
        {
            
        }

        
        protected override void GetBasicMovements()
        {
            BasicMovements = new List<Point>();

            for (int i = 1; i < 8; i++) 
            {
                BasicMovements.Add(new Point(i, i));
                BasicMovements.Add(new Point(i, -i));
                BasicMovements.Add(new Point(-i, i));
                BasicMovements.Add(new Point(-i, -i));
            }
            
        }
    }
}
