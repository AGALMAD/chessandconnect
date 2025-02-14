using chess4connect.DTOs.Games;
using chess4connect.Models.Games.Chess.Chess.Pieces.Base;

namespace chess4connect.Mappers;

public static class ChessPieceMapper
{
    public static ChessPieceDto ToDto(this ChessBasePiece piece)
    {
        return new ChessPieceDto
        {
            Id = piece.Id,
            Color = piece.Color,
            Position = piece.Position,
            PieceType = piece.PieceType
        };
    }

    public static List<ChessPieceDto> ToDto(this List<ChessBasePiece> pieces)
    {
        return pieces.Select(piece => piece.ToDto()).ToList();
    }
}
