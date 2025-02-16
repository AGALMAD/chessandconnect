using chess4connect.Enums;

namespace chess4connect.DTOs.Games;

public class RoomDto
{
    public int Player1Id { get; set; }
    public int? Player2Id { get; set; }
    public GameType GameType { get; set; }
}
