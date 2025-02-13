using System.ComponentModel.DataAnnotations.Schema;

namespace chess4connect.Models.Games.Base;

public class BaseBoard
{
    public List<BasePiece> Pieces { get; set; } = new List<BasePiece>();


}
