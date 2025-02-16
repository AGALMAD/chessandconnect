using chess4connect.DTOs.Games;
using chess4connect.Enums;
using chess4connect.Mappers;
using chess4connect.Models.Games;
using chess4connect.Models.Games.Chess.Chess;
using chess4connect.Models.Games.Chess.Chess.Pieces;
using chess4connect.Models.Games.Chess.Chess.Pieces.Base;
using chess4connect.Models.Games.Chess.Chess.Pieces.Types;
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


    public async Task<bool> MoveChessPiece(ChessMoveRequest moveRequest, int userId)
    {

        ChessRoom room = _roomService.GetChessRoomByUserId(userId);

        if (room.Game.Board.MovePiece(moveRequest))
        {
            await SendBoardMessageAsync(room.Player1Id, room.Player2Id, room.Game.GameType);
            return true;
        }

        return false;

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


    public async Task SendMovementsMessageAsync(int playerId)
    {
        //Sala de los jugadores
        ChessRoom room = _roomService.GetChessRoomByUserId(playerId);

        if (room != null)
        {
            WebSocketHandler socketPlayer = _network.GetSocketByUserId(playerId);

            //Recoge los movimientos que puede hacer el jugador
            if (room.Player1Id == playerId)
            {
                room.Game.Board.GetAllPieceMovements(ChessPieceColor.WHITE);
            }
            else
            {
                room.Game.Board.GetAllPieceMovements(ChessPieceColor.BLACK);
            }

            //Lista de piezas sin  los movimientos básicos
            var roomMessage = new SocketMessage<List<ChessPieceMovementDto>>
            {
                Type = SocketCommunicationType.CHESS_MOVEMENTS,

                Data = ChessPieceMovementsMappper.ToDto(room.Game.Board.ChessPiecesMovements)
            };

            string stringBoardMessage = JsonSerializer.Serialize(roomMessage);

            //Envia los mensajes a los jugadores
            if (socketPlayer != null)
            {
                await socketPlayer.SendAsync(stringBoardMessage);
            }

        }


       

    }


}
