using chess4connect.Models.Games.Base;
using System.Drawing;

namespace chess4connect.Models.Games;

public class MoveRequest
{
    public int PieceId { get; set; }
    public Point DestinationPosition { get; set; }

}
