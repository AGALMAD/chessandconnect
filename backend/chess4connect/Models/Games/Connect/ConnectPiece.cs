using chess4connect.Models.Games.Base;
using chess4connect.Models.Games.Chess.Chess.Pieces.Types;
using System.Drawing;

namespace chess4connect.Models.Games.Connect
{
    public class ConnectPiece
    {
        public bool Player1Piece { get; set; }
        public Point Position { get; set; }

        public ConnectPiece(bool pieceColor, Point position)
        {
            Player1Piece = pieceColor;
            Position = position;
        }
    }
}
