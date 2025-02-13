using chess4connect.Models.Games;

namespace chess4connect.Services;

public class GameService
{
    private readonly RoomService _roomService;

    public GameService(RoomService roomService)
    {
        _roomService = roomService;
    }


    public async Task MoveChessPiece(ChessMoveRequest moveRequest, int userId)
    {

        Game currentGame = _roomService.GetGameByUserId(userId);
        if (currentGame != null)
        {
            currentGame.Board.Move(moveRequest);
        }


    }


}
