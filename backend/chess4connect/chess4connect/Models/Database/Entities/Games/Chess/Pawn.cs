using chess4connect.Models.Database.Entities.Games.Base;

namespace chess4connect.Models.Database.Entities.Games.Chess
{
    public class Pawn : Piece
    {
        public Pawn(string color, int row, int column) : base(color, row, column) { }
    }
}
