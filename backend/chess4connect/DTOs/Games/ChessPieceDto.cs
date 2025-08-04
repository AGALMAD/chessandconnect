using chess4connect.Models.Games.Chess.Chess.Pieces.Types;
using System.Drawing;

namespace chess4connect.DTOs.Games;
public class ChessPieceDto
{
    public int Id { get; set; }
    public bool Player1Piece { get; set; }
    public Point Position { get; set; }
    public PieceType PieceType { get; set; }
}
