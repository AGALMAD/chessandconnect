using chess4connect.Enums;
using chess4connect.Models.Database.Entities.Games.Base;
using Microsoft.AspNetCore.Routing.Constraints;
using System.Drawing;
using System.Security.Cryptography.X509Certificates;

namespace chess4connect.Models.Database.Entities.Games.Chess
{
    public class ChessBoard : BaseBoard
    {
        public static int ROWS = 8;
        public static int COLUMNS = 8;


        public ChessBoard() : base(ROWS, COLUMNS)
        {
            InitalizeBoard();
        }

        private void InitalizeBoard()
        {
            // Inicializar piezas negras
            Board[0, 0] = new Rook(false, new Point(0, 0));
            Board[0, 1] = new Knight(false, new Point(0, 1));
            Board[0, 2] = new Bishop(false, new Point(0, 2));
            Board[0, 3] = new Queen(false, new Point(0, 3));
            Board[0, 4] = new King(false, new Point(0, 4));
            Board[0, 5] = new Bishop(false, new Point(0, 5));
            Board[0, 6] = new Knight(false, new Point(0, 6));
            Board[0, 7] = new Rook(false, new Point(0, 7));
            for (int i = 0; i < COLUMNS; i++)
            {
                Board[1, i] = new Pawn(false, new Point(1, i));
            }

            // Inicializar piezas blancas
            Board[7, 0] = new Rook(true, new Point(7, 0));
            Board[7, 1] = new Knight(true, new Point(7, 1));
            Board[7, 2] = new Bishop(true, new Point(7, 2));
            Board[7, 3] = new Queen(true, new Point(7, 3));
            Board[7, 4] = new King(true, new Point(7, 4));
            Board[7, 5] = new Bishop(true, new Point(7, 5));
            Board[7, 6] = new Knight(true, new Point(7, 6));
            Board[7, 7] = new Rook(true, new Point(7, 7));
            for (int i = 0; i < COLUMNS; i++)
            {
                Board[6, i] = new Pawn(true, new Point(6, i));

            }
        }

    }
}