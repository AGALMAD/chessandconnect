using chess4connect.DTOs.Games;
using chess4connect.Enums;
using chess4connect.Mappers;
using chess4connect.Models.Games;
using chess4connect.Models.Games.Chess.Chess;
using chess4connect.Models.Games.Chess.Chess.Pieces;
using chess4connect.Models.Games.Chess.Chess.Pieces.Base;
using chess4connect.Models.SocketComunication.Handlers;
using chess4connect.Models.SocketComunication.MessageTypes;
using System.Text.Json;

namespace chess4connect.Services;

public class GameService
{
    private readonly WebSocketNetwork _network;
    private readonly RoomService _roomService;

    public GameService(RoomService roomService, WebSocketNetwork network)
    {
        _roomService = roomService;
        _network = network;
    }


    public async Task MoveChessPiece(ChessMoveRequest moveRequest, int userId)
    {



    }



    private async Task SendBoardMessageAsync(int player1Id, int player2Id, GameType gamemode)
    {
        WebSocketHandler socketPlayer1 = _network.GetSocketByUserId(player1Id);
        WebSocketHandler socketPlayer2 = _network.GetSocketByUserId(player2Id);

        

    }



}
