using chess4connect.Models.Games;

namespace chess4connect.Services;

public class GameService
{
    private readonly RoomService _roomService;

    public GameService(RoomService roomService)
    {
        _roomService = roomService;
    }


    public async Task MovePiece(MoveRequest moveRequest)
    {

    }


}
