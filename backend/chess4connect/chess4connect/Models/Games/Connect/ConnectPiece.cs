using chess4connect.Models.Games.Base;
using chess4connect.Models.Games.Chess.Chess.Pieces.Types;
using System.Drawing;

namespace chess4connect.Models.Games.Connect
{
    public class ConnectPiece: BasePiece
    {
        public ConnectPiece(int id, PieceColor color, Point position) : base(id,color,position) { }
    }
}
