using chess4connect.DTOs.Games;
using chess4connect.Models.Games.Chess.Chess;
using chess4connect.Models.Games.Chess.Chess.Pieces.Base;

namespace chess4connect.Mappers;

public class ChessPieceMovementsMappper
{
    public static ChessPieceMovementDto ToDto(ChessPiecesMovements movement)
    {
        return new ChessPieceMovementDto
        {
            Id = movement.Piece.Id,
            Movements = movement.Movements,
        };
    }

    public static List<ChessPieceMovementDto> ToDto(List<ChessPiecesMovements> movements)
    {
        return movements.Select(movement => ChessPieceMovementsMappper.ToDto(movement)).ToList();
    }
}
