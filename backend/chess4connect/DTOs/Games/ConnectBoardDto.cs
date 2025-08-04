using chess4connect.Models.Games.Chess.Chess.Pieces.Types;
using chess4connect.Models.Games.Connect;

namespace chess4connect.DTOs.Games;

public class ConnectBoardDto
{
    public ConnectPiece LastPiece { get; set; }
    public bool Player1Turn { get; set; }
    public int Player1Time { get; set; }
    public int Player2Time { get; set; }
}
