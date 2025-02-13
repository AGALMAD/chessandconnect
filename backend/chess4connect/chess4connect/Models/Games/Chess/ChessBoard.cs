using chess4connect.Models.Games.Base;
using chess4connect.Models.Games.Chess.Pieces;
using chess4connect.Models.Games.Chess.Pieces.Types;
using System.Drawing;

namespace chess4connect.Models.Games.Chess
{
    public class ChessBoard : BaseBoard
    {
        public static int ROWS = 8;
        public static int COLUMNS = 8;


        public ChessBoard()
        {
            Pieces = PlacePiecesInBoard();
        }

        private List<BasePiece> PlacePiecesInBoard()
        {
            List<BasePiece> allPieces = new List<BasePiece>
            {
                new Rook(8, ChessPieceColor.BLACK, new Point(0, 0)),
                new Knight(9, ChessPieceColor.BLACK, new Point(0, 1)),
                new Bishop(10, ChessPieceColor.BLACK, new Point(0, 2)),
                new Queen(11, ChessPieceColor.BLACK, new Point(0, 3)),
                new King(12, ChessPieceColor.BLACK, new Point(0, 4)),
                new Bishop(13, ChessPieceColor.BLACK, new Point(0, 5)),
                new Knight(14, ChessPieceColor.BLACK, new Point(0, 6)),
                new Rook(15, ChessPieceColor.BLACK, new Point(0, 7)),

                new Rook(24, ChessPieceColor.WHITE, new Point(7, 0)),
                new Knight(25, ChessPieceColor.WHITE, new Point(7, 1)),
                new Bishop(26, ChessPieceColor.WHITE, new Point(7, 2)),
                new Queen(27, ChessPieceColor.WHITE, new Point(7, 3)),
                new King(28, ChessPieceColor.WHITE, new Point(7, 4)),
                new Bishop(29, ChessPieceColor.WHITE, new Point(7, 5)),
                new Knight(30, ChessPieceColor.WHITE, new Point(7, 6)),
                new Rook(31, ChessPieceColor.WHITE, new Point(7, 7))
            };

            for (int i = 0; i < COLUMNS; i++)
            {
                allPieces.Add(new Pawn(16 + i, ChessPieceColor.BLACK, new Point(1, i)));
            }
            for (int i = 0; i < COLUMNS; i++)
            {
                allPieces.Add(new Pawn(32 + i, ChessPieceColor.WHITE, new Point(6, i)));
            }

            return allPieces;
        }


    }
}