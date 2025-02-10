using chess4connect.Enums;
using Microsoft.AspNetCore.Routing.Constraints;
using System.Security.Cryptography.X509Certificates;

namespace chess4connect.Models.Database.Entities.Games.Chess
{
    public class ChessBoard
    {
        public static int ROWS = 8;
        public static int COLUMS = 8;

        public Piece[,] Board { get; set; } = new Piece[ROWS, COLUMS];

        public ChessBoard()
        {
            InitalizeBoard();
        }

        private void InitalizeBoard()
        {
            //TODO colocar piezas
        }
    }
}
