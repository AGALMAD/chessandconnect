using System.ComponentModel.DataAnnotations.Schema;

namespace chess4connect.Models.Games.Base;

public class BaseBoard
{
    [NotMapped]
    public BasePiece[,] Board { get; set; }


    public List<BasePiece> GetAllPieces()
    {
        var pieces = new List<BasePiece>();

        foreach (var piece in Board)
        {
            if (piece != null)
                pieces.Add(piece);
        }

        return pieces;
    }


    public void SetPieces(List<BasePiece> pieces)
    {
        foreach (var piece in pieces)
        {
            Board[piece.Position.X, piece.Position.Y] = piece;
        }
    }
}
