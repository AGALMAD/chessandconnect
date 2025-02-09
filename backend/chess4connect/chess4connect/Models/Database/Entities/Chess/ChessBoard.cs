using chess4connect.Enums;
using Microsoft.AspNetCore.Routing.Constraints;
using System.Security.Cryptography.X509Certificates;

namespace chess4connect.Models.Database.Entities.Chess
{
    public class ChessBoard
    {
        public int Rows { get; set; } = 8;
        public int Columns { get; set; } = 8;

        public List<Piece> Pieces { get; set; }

        public ChessBoard()
        {
            Pieces = new List<Piece>();
            InitalizeBoard();
        }

        private void InitalizeBoard()
        {
            //TODO colocar piezas
        }
    }
}
