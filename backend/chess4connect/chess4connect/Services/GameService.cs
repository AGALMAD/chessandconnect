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

    public async Task<bool> MoveChessPiece(ChessRoom room, ChessMoveRequest moveRequest, WebSocketHandler player, WebSocketHandler opponent)
    {


        if (room.Game.Board.MovePiece(moveRequest))
        {
            await SendChessBoardMessageAsync(room, player, opponent);

            await SendMovementsMessageAsync(room, player);

            return true;
        }

        return false;

    }



    public async Task SendChessBoardMessageAsync(ChessRoom room, WebSocketHandler player1, WebSocketHandler player2)
    {


        if (room != null)
        {
            //Lista de piezas original
            List<ChessBasePiece> pieces = room.Game.Board.convertBoardToList();

            //Lista de piezas sin  los movimientos básicos
            var roomMessage = new SocketMessage<ChessBoardDto>
            {
                Type = SocketCommunicationType.CHESS_BOARD,

                Data = new ChessBoardDto
                {
                    Pieces = ChessPieceMapper.ToDto(pieces),
                    Turn = room.Game.Board.Turn,
                    Player1Time = (int)room.Game.Board.Player1Time.TotalSeconds,
                    Player2Time = (int)room.Game.Board.Player2Time.TotalSeconds,

                }
            };

            string stringBoardMessage = JsonSerializer.Serialize(roomMessage);
            //Envia los mensajes a los jugadores
            if (player1 != null)
            {
                await player1.SendAsync(stringBoardMessage);
            }

            if (player1 != null)
            {
                await player1.SendAsync(stringBoardMessage);
            }


        }


    }


    public async Task SendMovementsMessageAsync(ChessRoom room, WebSocketHandler player)
    {

        if (room != null)
        {

            //Recoge los movimientos que puede hacer el jugador
            if (room.Player1Id == player.Id)
            {
                room.Game.Board.GetAllPieceMovements();
            }
            else
            {
                room.Game.Board.GetAllPieceMovements();
            }

            //Lista de piezas sin  los movimientos básicos
            var roomMessage = new SocketMessage<List<ChessPieceMovementDto>>
            {
                Type = SocketCommunicationType.CHESS_MOVEMENTS,

                Data = ChessPieceMovementsMappper.ToDto(room.Game.Board.ChessPiecesMovements)
            };

            string stringBoardMessage = JsonSerializer.Serialize(roomMessage);

            //Envia los movimientos al jugador
            if (player != null)
            {
                await player.SendAsync(stringBoardMessage);
            }

        }


       

    }


}
