using chess4connect.Models.Games.Base;
using chess4connect.Models.Games.Chess.Chess.Pieces.Base;
using System.Drawing;

namespace chess4connect.Models.Games.Connect;

public class ConnectBoard
{
    public static int ROWS = 6;
    public static int COLUMNS = 7;

    private BasePiece[,] Board = new BasePiece[ROWS, COLUMNS];

}


