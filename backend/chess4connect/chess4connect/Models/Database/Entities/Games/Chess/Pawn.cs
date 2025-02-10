using chess4connect.Models.Database.Entities.Games.Base;
using System.Drawing;

namespace chess4connect.Models.Database.Entities.Games.Chess
{
    public class Pawn : BasePiece
    {
        public Pawn(bool host, Point position) : base(host, position) { }
    }
}
