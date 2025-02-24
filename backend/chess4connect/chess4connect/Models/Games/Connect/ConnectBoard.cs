using chess4connect.Models.Games.Base;
using chess4connect.Models.Games.Chess.Chess.Pieces.Base;
using chess4connect.Models.Games.Chess.Chess.Pieces.Types;
using System.Drawing;

namespace chess4connect.Models.Games.Connect;

public class ConnectBoard
{
    public static int COLUMNS = 7;
    public static int ROWS = 6;

    private ConnectPiece[,] Board = new ConnectPiece[COLUMNS, ROWS];


    public bool Player1Turn { get; set; } = true;
    public int turnsCounter {  get; set; } = 1;


    //Tiempo en segundo de cada turno
    public TimeSpan Player1Time { get; set; } = TimeSpan.FromSeconds(300);
    public TimeSpan Player2Time { get; set; } = TimeSpan.FromSeconds(300);

    //Fecha de inicio de cada turno
    public DateTime StartTurnDateTime { get; set; }

    public int DropPiece(int colum)
    {
        for (int i = ROWS; i > 0; i--)
        {
            if (colum <= COLUMNS && Board[colum,i] == null)
            {
                Board[colum -1, i] = new ConnectPiece(Player1Turn, new Point(colum -1,i));

            }

            Player1Turn = !Player1Turn;
            turnsCounter++; 

            if( turnsCounter >= 8 && CheckVictory(colum, i))
            {
                return 1;
            }


            return 0;
        }

        return -1;
    }



    public bool CheckVictory(int col, int row)
    {
        if (Board[col, row] == null)
            return false;

        bool color = Board[col, row].Player1Piece;

        return CheckDirection(col, row, 1, 0, color) ||  // Horizontal
               CheckDirection(col, row, 0, 1, color) ||  // Vertical
               CheckDirection(col, row, 1, 1, color) ||  // Diagonal ↘
               CheckDirection(col, row, 1, -1, color);   // Diagonal ↗
    }

    private bool CheckDirection(int col, int row, int dCol, int dRow, bool color)
    {
        int count = 1;

        //verifica en 2 direcciones
        count += CountPieces(col, row, dCol, dRow, color);
        count += CountPieces(col, row, -dCol, -dRow, color);

        return count >= 4;
    }

    private int CountPieces(int col, int row, int dCol, int dRow, bool color)
    {
        int count = 0;

        for (int i = 1; i < 4; i++)
        {
            int newCol = col + dCol * i;
            int newRow = row + dRow * i;

            if (newCol < 0 || newCol >= COLUMNS || newRow < 0 || newRow >= ROWS)
                break;

            if (Board[newCol, newRow] == null || Board[newCol, newRow].Player1Piece != color)
                break;

            count++;
        }

        return count;
    }




    public List<ConnectPiece> convertBoardToList()
    {
        List<ConnectPiece> piecesInBoard = new List<ConnectPiece>();

        for (int i = 0; i < Board.GetLength(0); i++)
        {
            for (int j = 0; j < Board.GetLength(1); j++)
            {
                if (Board[i, j] != null)
                    piecesInBoard.Add(Board[i, j]);
            }
        }


        return piecesInBoard;

    }
}


