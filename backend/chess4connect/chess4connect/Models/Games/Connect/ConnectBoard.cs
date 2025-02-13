using chess4connect.Models.Games.Base;
using System.Drawing;

namespace chess4connect.Models.Games.Connect;

public class ConnectBoard : BaseBoard
{
    public static int ROWS = 6;
    public static int COLUMNS = 7;
    public ConnectBoard()
    {

        Pieces = new List<BasePiece>();
    }
}


