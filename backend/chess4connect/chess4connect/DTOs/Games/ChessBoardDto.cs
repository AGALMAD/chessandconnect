using chess4connect.Models.Games.Chess.Chess;
using chess4connect.Models.Games.Chess.Chess.Pieces.Types;

namespace chess4connect.DTOs.Games;

public class ChessBoardDto
{    
    public List<ChessPieceDto> Pieces { get; set; }
    public bool Player1Turn { get; set; }
    public int Player1Time { get; set; }
    public int Player2Time { get; set; }

}
