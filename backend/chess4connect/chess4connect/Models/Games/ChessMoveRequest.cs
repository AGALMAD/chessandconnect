using chess4connect.Models.Games.Base;
using System.Drawing;

namespace chess4connect.Models.Games;

public class ChessMoveRequest
{
    public int PieceId { get; set; }
    public int MovementX { get; set; }
    public int MovementY { get; set; }


}
