using System.Drawing;

namespace chess4connect.DTOs.Games;

public class ChessPieceMovementDto
{
    public int Id { get; set; }
    public List<Point> Movements { get; set; }

}
