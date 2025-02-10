using System.ComponentModel.DataAnnotations.Schema;

namespace chess4connect.Models.Games.Base;

public class BaseBoard
{
    [NotMapped]
    public BasePiece[,] Board { get; set; }

}
