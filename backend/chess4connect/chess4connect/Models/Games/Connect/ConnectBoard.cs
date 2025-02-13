using chess4connect.Models.Games.Base;
using System.Drawing;

namespace chess4connect.Models.Games.Connect;

public class ConnectBoard : IBoard
{
    public static int ROWS = 6;
    public static int COLUMNS = 7;
    public ConnectBoard()
    {

        Pieces = new List<IPiece>();
    }

    public List<IPiece> Pieces { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public void Initialize()
    {
        throw new NotImplementedException();
    }
}


