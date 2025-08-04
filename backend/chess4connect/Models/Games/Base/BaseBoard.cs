using System.ComponentModel.DataAnnotations.Schema;

namespace chess4connect.Models.Games.Base;

public class BaseBoard<TPiece>
{
    public List<TPiece> Pieces { get; set; } = new List<TPiece>();


}
