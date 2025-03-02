using chess4connect.Models.Games.Base;
using chess4connect.Models.Games.Chess.Chess.Pieces.Base;
using chess4connect.Models.Games.Chess.Chess.Pieces.Types;
using System.Drawing;

namespace chess4connect.Models.Games.Connect;

public class ConnectBoard
{
    public static int COLUMNS = 7;
    public static int ROWS = 6;

    private ConnectPiece[,] Board = new ConnectPiece[ROWS, COLUMNS];


    public bool Player1Turn { get; set; } = true;
    public int turnsCounter { get; set; } = 1;

    public ConnectPiece LastPiece { get; set; }

    // Tiempo en segundos de cada turno
    public TimeSpan Player1Time { get; set; } = TimeSpan.FromSeconds(180);
    public TimeSpan Player2Time { get; set; } = TimeSpan.FromSeconds(180);

    // Fecha de inicio de cada turno
    public DateTime StartTurnDateTime { get; set; }

    public int DropPiece(int colum)
    {
        if (colum < 0 || colum > COLUMNS)
            return -1;


        for (int i = ROWS - 1; i >= 0; i--)
        {
            if (Board[i, colum] == null)
            {
                ConnectPiece piece = new ConnectPiece(Player1Turn, new Point(colum, i));
                Board[i, colum] = piece;
                LastPiece = piece;

                turnsCounter++;

                // Update time
                TimeSpan timeSpent = DateTime.Now.Subtract(StartTurnDateTime);
                if (Player1Turn)
                    Player1Time -= timeSpent;
                else
                    Player2Time -= timeSpent;


                if (turnsCounter >= 8 && CheckVictory(colum, i))
                {
                    return 1;
                }

                Player1Turn = !Player1Turn;
                StartTurnDateTime = DateTime.Now;

                return 0;
            }
        }

        return -1;  // Si la columna está llena
    }

    public bool CheckVictory(int col, int row)
    {
        if (Board[row, col] == null)
            return false;

        bool color = Board[row, col].Player1Piece;

        return CheckDirection(row, col, 1, 0, color) ||  // Horizontal
               CheckDirection(row, col, 0, 1, color) ||  // Vertical
               CheckDirection(row, col, 1, 1, color) ||  // Diagonal ↘
               CheckDirection(row, col, 1, -1, color);   // Diagonal ↗
    }

    private bool CheckDirection(int row, int col, int dRow, int dCol, bool color)
    {
        int count = 1;

        // Verifica en ambas direcciones
        count += CountPieces(row, col, dRow, dCol, color);
        count += CountPieces(row, col, -dRow, -dCol, color);

        return count >= 4;
    }

    private int CountPieces(int row, int col, int dRow, int dCol, bool color)
    {
        int count = 0;

        for (int i = 1; i < 4; i++)
        {
            int newRow = row + dRow * i;
            int newCol = col + dCol * i;

            if (newRow < 0 || newRow >= ROWS || newCol < 0 || newCol >= COLUMNS)
                break;

            if (Board[newRow, newCol] == null || Board[newRow, newCol].Player1Piece != color)
                break;

            count++;
        }

        return count;
    }

    public async Task RandomDrop()
    {
        Random random = new Random();

        await Task.Delay(random.Next(1000, 5000));

        while (DropPiece(random.Next(0, 7)) == -1) ;
    }
}

