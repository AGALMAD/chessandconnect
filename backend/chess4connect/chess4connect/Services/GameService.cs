using chess4connect.DTOs.Games;
using chess4connect.Enums;
using chess4connect.Models.Games;
using chess4connect.Models.SocketComunication.Handlers;
using chess4connect.Models.SocketComunication.MessageTypes;
using System.Text.Json;

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



    }


    private async Task SendBoardMessageAsync(WebSocketHandler socketPlayer1, WebSocketHandler socketPlayer2)
    {
        var game = _roomService.GetGameByUserId(socketPlayer1.Id);

        var socketMessage = new SocketMessage<>
        {
            Type = SocketCommunicationType.GAME_START,
            Data = new RoomDto
            {
                Player1Id = room.Player1Id,
                Player2Id = room.Player2Id,
                GameType = room.Game.GameType,
            },
        };

        string stringSocketMessage = JsonSerializer.Serialize(socketMessage);


        await socketPlayer1.SendAsync(stringSocketMessage);
        if (socketPlayer2 is not null)
        {
            await socketPlayer2.SendAsync(stringSocketMessage);
        }
    }


}
