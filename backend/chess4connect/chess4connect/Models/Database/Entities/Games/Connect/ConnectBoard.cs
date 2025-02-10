using System.Drawing;

namespace chess4connect.Models.Database.Entities.Games.Connect;

public class ConnectBoard
{
    public static int ROWS = 6;
    public static int COLUMS = 7;

    public Piece[,] Board { get; set; } = new Piece[ROWS, COLUMS];
}


