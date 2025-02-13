using System.ComponentModel.DataAnnotations.Schema;

namespace chess4connect.Models.Games.Base;

public interface IBoard
{
    public List<IPiece> Pieces { get; set; }
    void Initialize();


}
