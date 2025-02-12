using chess4connect.Models.Games.Base;
using System.Drawing;

namespace chess4connect.Models.Games.Chess.Pieces
{
    public class Pawn : BasePiece
    {
        public Pawn(int id, Chess.Color color, Point position) : base(id, color, position) { }
    }
}
