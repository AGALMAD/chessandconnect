using chess4connect.DTOs.Games;
using chess4connect.Enums;
using chess4connect.Mappers;
using chess4connect.Models.Games;
using chess4connect.Models.Games.Chess.Chess;
using chess4connect.Models.Games.Chess.Chess.Pieces;
using chess4connect.Models.Games.Chess.Chess.Pieces.Base;
using chess4connect.Models.SocketComunication.Handlers;
using chess4connect.Models.SocketComunication.MessageTypes;
using System.Security.AccessControl;
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



    public async Task SendBoardMessageAsync(int player1Id, int player2Id, GameType gamemode)
    {
        WebSocketHandler socketPlayer1 = _network.GetSocketByUserId(player1Id);
        WebSocketHandler socketPlayer2 = _network.GetSocketByUserId(player2Id);

        string stringBoardMessage = "";

        if (gamemode == GameType.Chess)
        {
            //Sala de los jugadores
            ChessRoom room = _roomService.GetChessRoomByUserId(player1Id);

            if (room != null)
            {
                //Lista de piezas original
                List<ChessBasePiece> pieces = room.Game.Board.convertBoardToList();

                //Lista de piezas sin  los movimientos básicos
                var roomMessage = new SocketMessage<List<ChessPieceDto>>
                {
                    Type = SocketCommunicationType.CHESS_BOARD,

                    Data = ChessPieceMapper.ToDto(pieces)
                };

                stringBoardMessage = JsonSerializer.Serialize(roomMessage);

            }


        }


        //Envia los mensajes a los jugadores
        if (socketPlayer1 != null)
        {
            await socketPlayer1.SendAsync(stringBoardMessage);
        }

        if (socketPlayer2 != null)
        {
            await socketPlayer2.SendAsync(stringBoardMessage);
        }

    }



}
