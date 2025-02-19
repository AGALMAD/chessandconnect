using chess4connect.Models.Games.Chess.Chess.Pieces;
using chess4connect.Models.Games.Chess.Chess.Pieces.Base;
using chess4connect.Models.Games.Chess.Chess.Pieces.Types;
using System.Drawing;

namespace chess4connect.Models.Games.Chess.Chess
{
    public class ChessBoard
    {
        public static int ROWS = 8;
        public static int COLUMNS = 8;

        public List<ChessPiecesMovements> ChessPiecesMovements { get; set; }

        private ChessBasePiece[,] Board = new ChessBasePiece[ROWS, COLUMNS];

        public ChessPieceColor Turn { get; set; }


        //Tiempo en segundo de cada turno
        public TimeSpan Player1Time { get; set; } = TimeSpan.FromSeconds(300);
        public TimeSpan Player2Time { get; set; } = TimeSpan.FromSeconds(300);

        //Fecha de inicio de cada turno
        public DateTime StartTurnDateTime { get; set; }



        public ChessBoard()
        {
            PlacePiecesInBoard();
        }

        private void PlacePiecesInBoard()
        {
            Board = new ChessBasePiece[ROWS, COLUMNS];

            //Black pieces initialized in Board
            Board[0, 0] = new Rook(8, PieceColor.BLACK, new Point(0, 0));
            Board[0, 1] = new Knight(9, PieceColor.BLACK, new Point(0, 1));
            Board[0, 2] = new Bishop(10, PieceColor.BLACK, new Point(0, 2));
            Board[0, 3] = new Queen(11, PieceColor.BLACK, new Point(0, 3));
            Board[0, 4] = new King(12, PieceColor.BLACK, new Point(0, 4));
            Board[0, 5] = new Bishop(13, PieceColor.BLACK, new Point(0, 5));
            Board[0, 6] = new Knight(14, PieceColor.BLACK, new Point(0, 6));
            Board[0, 7] = new Rook(15, PieceColor.BLACK, new Point(0, 7));

            for (int i = 0; i < COLUMNS; i++)
            {
                Board[1, i] = new Pawn(16 + i, PieceColor.BLACK, new Point(1, i));
            }


            //White pieces initialized in Board
            Board[7, 0] = new Rook(24, PieceColor.WHITE, new Point(7, 0));
            Board[7, 1] = new Knight(25, PieceColor.WHITE, new Point(7, 1));
            Board[7, 2] = new Bishop(26, PieceColor.WHITE, new Point(7, 2));
            Board[7, 3] = new Queen(27, PieceColor.WHITE, new Point(7, 3));
            Board[7, 4] = new King(28, PieceColor.WHITE, new Point(7, 4));
            Board[7, 5] = new Bishop(29, PieceColor.WHITE, new Point(7, 5));
            Board[7, 6] = new Knight(30, PieceColor.WHITE, new Point(7, 6));
            Board[7, 7] = new Rook(31, PieceColor.WHITE, new Point(7, 7));

            for (int i = 0; i < COLUMNS; i++)
            {
                Board[6, i] = new Pawn(32 + i, PieceColor.WHITE, new Point(6, i));
            }


        }


        public void GetAllPieceMovements()
        {
            ChessPiecesMovements = new List<ChessPiecesMovements>();

            foreach (ChessBasePiece piece in Board)
            {
                if (piece == null || piece.Color != Turn) continue;

                List<Point> movementList = new List<Point>();

                switch (piece.PieceType)
                {
                    case PieceType.PAWN:
                        CalculatePawnMoves(piece, movementList);
                        break;

                    case PieceType.KNIGHT:
                        CalculateKnightMoves(piece, movementList);
                        break;

                    case PieceType.KING:
                        CalculateKingMoves(piece, movementList);
                        break;

                    case PieceType.ROOK:
                    case PieceType.BISHOP:
                    case PieceType.QUEEN:
                        CalculateSlidingMoves(piece, movementList);
                        break;
                }

                ChessPiecesMovements newMovements = new ChessPiecesMovements
                {
                    Piece = new ChessPieceWhithOutBasicMovements
                    {
                        Id = piece.Id,
                        Color = piece.Color,
                        PieceType = piece.PieceType,
                        Position = piece.Position,
                    },
                    Movements = movementList
                };

                ChessPiecesMovements.Add(newMovements);
            }
        }

        private void CalculatePawnMoves(ChessBasePiece piece, List<Point> movementList)
        {
            foreach (Point move in piece.BasicMovements)
            {
                int newX = piece.Position.X + move.X;
                int newY = piece.Position.Y + move.Y;

                // Early bounds check
                if (newX < 0 || newX >= 8 || newY < 0 || newY >= 8)
                    continue;

                // Forward movement (when Y doesn't change)
                if (move.Y == 0)
                {

                    // Can't move forward if piece is blocking
                    if (Board[newX, newY] != null)
                        continue;


                    // For two-square first move, check if path is clear
                    if (Math.Abs(move.X) == 2)
                    {
                        // Fix: Calculate intermediate square based on direction
                        int direction = piece.Color == ChessPieceColor.WHITE ? -1 : 1;
                        int intermediateX = piece.Position.X + direction;

                        if (Board[intermediateX, newY] != null)
                            continue;
                    }


                    movementList.Add(new Point(newX, newY));
                }
                // Diagonal captures
                else if (Board[newX, newY]?.Color != piece.Color && Board[newX, newY] != null)
                {
                    movementList.Add(new Point(newX, newY));
                }
            }
        }

        private void CalculateKnightMoves(ChessBasePiece piece, List<Point> movementList)
        {
            foreach (Point move in piece.BasicMovements)
            {
                int newX = piece.Position.X + move.X;
                int newY = piece.Position.Y + move.Y;


                if (newX >= 0 && newX < 8 && newY >= 0 && newY < 8)
                {
                    if (Board[newX, newY] == null || Board[newX, newY].Color != piece.Color)
                    {
                        movementList.Add(new Point(newX, newY));
                    }
                }
            }
        }

        private void CalculateKingMoves(ChessBasePiece piece, List<Point> movementList)
        {
            foreach (Point move in piece.BasicMovements)
            {
                int newX = piece.Position.X + move.X;
                int newY = piece.Position.Y + move.Y;

                if (newX >= 0 && newX < 8 && newY >= 0 && newY < 8)
                {
                    if (Board[newX, newY] == null || Board[newX, newY].Color != piece.Color)
                    {
                        movementList.Add(new Point(newX, newY));
                    }
                }
            }
        }

        private void CalculateSlidingMoves(ChessBasePiece piece, List<Point> movementList)
        {
            foreach (Point direction in piece.BasicMovements)
            {
                for (int distance = 1; distance < 8; distance++)
                {
                    int newX = piece.Position.X + (direction.X * distance);
                    int newY = piece.Position.Y + (direction.Y * distance);

                    if (newX < 0 || newX >= 8 || newY < 0 || newY >= 8)
                        break;

                    if (Board[newX, newY] != null)
                    {
                        if (Board[newX, newY].Color != piece.Color)
                            movementList.Add(new Point(newX, newY));
                        break;
                    }

                    movementList.Add(new Point(newX, newY));
                }
            }
        }


        public int MovePiece(ChessMoveRequest moveRequest)
        {
            // Find the piece
            var piece = convertBoardToList().FirstOrDefault(p => p.Id == moveRequest.PieceId);
            if (piece == null) return -1;


            // Verify if movement is valid
            var chessPieceMovements = ChessPiecesMovements.FirstOrDefault(p => p.Piece.Id == piece.Id);
            if (chessPieceMovements == null ||
                !chessPieceMovements.Movements.Contains(new Point(moveRequest.MovementX, moveRequest.MovementY)))
            {
                return -1;
            }


            // Store the old position
            Point oldPosition = piece.Position;

            // Execute the move
            Board[oldPosition.X, oldPosition.Y] = null;

            // Pawn promotion
            if (piece.PieceType == PieceType.PAWN &&
                ((piece.Color == ChessPieceColor.WHITE && moveRequest.MovementX == 0) ||
                 (piece.Color == ChessPieceColor.BLACK && moveRequest.MovementX == ROWS - 1)))
            {
                // Create new queen and update board
                var queen = new Queen(piece.Id, piece.Color, new Point(moveRequest.MovementX, moveRequest.MovementY));
                Board[moveRequest.MovementX, moveRequest.MovementY] = queen;
                // Update the piece to the new queen
                piece = queen;
            }
            else
            {
                // Normal move
                Board[moveRequest.MovementX, moveRequest.MovementY] = piece;
                piece.Position = new Point(moveRequest.MovementX, moveRequest.MovementY);

                // Update FirstMove flag for pawns
                if (piece.PieceType == PieceType.PAWN && piece is Pawn pawn)
                {
                    pawn.FirstMove = false;
                }
            }

            // Update time
            TimeSpan timeSpent = DateTime.Now.Subtract(StartTurnDateTime);
            if (piece.Color == ChessPieceColor.WHITE)
                Player1Time -= timeSpent;
            else
                Player2Time -= timeSpent;

            // Change turn
            Turn = Turn == ChessPieceColor.BLACK ? ChessPieceColor.WHITE : ChessPieceColor.BLACK;
            StartTurnDateTime = DateTime.Now;

            // Recalculate all possible moves for the new position
            GetAllPieceMovements();

            // Check if the move results in checkmate
            if (IsCheckmate())
            {
                return 1;
            }

            return 0;
        }



        // check for checkmate
        private bool IsCheckmate()
        {
            // Get the opponent's king
            var opponentColor = Turn;
            var king = convertBoardToList()
                .FirstOrDefault(p => p.PieceType == PieceType.KING && p.Color == opponentColor);

            if (king == null) return false;

            // If king has no valid moves and is under attack, it's checkmate
            var kingMoves = ChessPiecesMovements
                .FirstOrDefault(p => p.Piece.Id == king.Id);

            // Check if king is under attack and has no valid moves
            return IsKingUnderAttack(king) &&
                   (kingMoves == null || !kingMoves.Movements.Any());
        }

        private bool IsKingUnderAttack(ChessBasePiece king)
        {
            // Change turn temporarily to calculate opponent's moves
            var originalTurn = Turn;
            Turn = Turn == ChessPieceColor.BLACK ? ChessPieceColor.WHITE : ChessPieceColor.BLACK;

            GetAllPieceMovements();


            // Check if any opponent piece can capture the king's position
            bool isUnderAttack = ChessPiecesMovements
                .Any(p => p.Movements.Contains(king.Position));

            // Restore original turn
            Turn = originalTurn;
            GetAllPieceMovements();

            return isUnderAttack;
        }



        public void RandomMovement()
        {
            // Calculate possible movements
            GetAllPieceMovements();

            // Get pieces valid moves
            var playerPiecesMovements = ChessPiecesMovements
                .Where(p => p.Piece.Color == Turn && p.Movements.Any())
                .ToList();

            // If no valid moves available, return
            if (!playerPiecesMovements.Any())
            {
                return;
            }

            var random = new Random();
            int maxAttempts = 100; 
            int attempts = 0;


            while (attempts < maxAttempts)
            {
                try
                {
                    // Select a random piece that has valid moves
                    var selectedPieceMovement = playerPiecesMovements[random.Next(playerPiecesMovements.Count)];

                    // Make sure the piece has valid moves
                    if (selectedPieceMovement.Movements.Count == 0)
                    {
                        attempts++;
                        continue;
                    }

                    // Select a random move
                    var randomMove = selectedPieceMovement.Movements[random.Next(selectedPieceMovement.Movements.Count)];


                    // Try to make the move
                    int result = MovePiece(new ChessMoveRequest
                    {
                        PieceId = selectedPieceMovement.Piece.Id,
                        MovementX = randomMove.X,
                        MovementY = randomMove.Y,
                    });

                    if (result == 0)
                    {
                        // Move successful
                        return;
                    }

                    attempts++;
                }
                catch (Exception)
                {
                    attempts++;
                }
            }
        }

        public List<ChessBasePiece> convertBoardToList()
        {
            List<ChessBasePiece> piecesInBoard = new List<ChessBasePiece>();

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
}