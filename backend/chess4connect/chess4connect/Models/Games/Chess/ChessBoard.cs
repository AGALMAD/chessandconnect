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

        public bool Player1Turn { get; set; }


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
            Board[0, 0] = new Rook(8, false, new Point(0, 0));
            Board[0, 1] = new Knight(9, false, new Point(0, 1));
            Board[0, 2] = new Bishop(10, false, new Point(0, 2));
            Board[0, 3] = new Queen(11, false, new Point(0, 3));
            Board[0, 4] = new King(12, false, new Point(0, 4));
            Board[0, 5] = new Bishop(13, false, new Point(0, 5));
            Board[0, 6] = new Knight(14, false, new Point(0, 6));
            Board[0, 7] = new Rook(15, false, new Point(0, 7));

            for (int i = 0; i < COLUMNS; i++)
            {
                Board[1, i] = new Pawn(16 + i, false, new Point(1, i));
            }


            //White pieces initialized in Board
            Board[7, 0] = new Rook(24, true, new Point(7, 0));
            Board[7, 1] = new Knight(25, true, new Point(7, 1));
            Board[7, 2] = new Bishop(26, true, new Point(7, 2));
            Board[7, 3] = new Queen(27, true, new Point(7, 3));
            Board[7, 4] = new King(28, true, new Point(7, 4));
            Board[7, 5] = new Bishop(29, true, new Point(7, 5));
            Board[7, 6] = new Knight(30, true, new Point(7, 6));
            Board[7, 7] = new Rook(31, true, new Point(7, 7));

            for (int i = 0; i < COLUMNS; i++)
            {
                Board[6, i] = new Pawn(32 + i, true, new Point(6, i));
            }


        }


        public void GetAllPieceMovements()
        {
            ChessPiecesMovements = new List<ChessPiecesMovements>();

            foreach (ChessBasePiece piece in Board)
            {
                if (piece == null || piece.Player1Piece != Player1Turn) continue;

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
                        Player1Color = piece.Player1Piece,
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
                        int direction = piece.Player1Piece ? -1 : 1;
                        int intermediateX = piece.Position.X + direction;

                        if (Board[intermediateX, newY] != null)
                     
                            continue;
                    }


                    movementList.Add(new Point(newX, newY));
                }
                // Diagonal captures
                else if (Board[newX, newY]?.Player1Piece != piece.Player1Piece && Board[newX, newY] != null)
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
                    if (Board[newX, newY] == null || Board[newX, newY].Player1Piece != piece.Player1Piece)
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
                    if (Board[newX, newY] == null || Board[newX, newY].Player1Piece != piece.Player1Piece)
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
                        if (Board[newX, newY].Player1Piece != piece.Player1Piece)
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
                ((piece.Player1Piece && moveRequest.MovementX == 0) ||
                 (piece.Player1Piece && moveRequest.MovementX == ROWS - 1)))
            {
                // Create new queen and update board
                var queen = new Queen(piece.Id, piece.Player1Piece, new Point(moveRequest.MovementX, moveRequest.MovementY));
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
            if (piece.Player1Piece)
                Player1Time -= timeSpent;
            else
                Player2Time -= timeSpent;

            
            // Check if the move results in checkmate
            if (IsCheckmate())
            {
                return 1;
            }

            // Change turn
            Player1Turn = !Player1Turn;
            StartTurnDateTime = DateTime.Now;

            // Recalculate all possible moves for the new position
            GetAllPieceMovements();


            return 0;
        }



        // check for checkmate
        private bool IsCheckmate()
        {
            // Get the opponent's king
            var opponentColor = !Player1Turn;
            var king = convertBoardToList().FirstOrDefault(p => p.PieceType == PieceType.KING && p.Player1Piece == opponentColor);

            if (king == null) return false;

            // Check if the king is under attack
            if (!IsKingUnderAttack(king)) return false;

            // Get possible moves for the king
            var kingMoves = new List<Point>();
            CalculateKingMoves(king, kingMoves);

            // If the king can move to a safe position, it's not checkmate
            foreach (var move in kingMoves)
            {
                // Temporarily move the king to the new position
                var originalPosition = king.Position;
                king.Position = move;

                if (!IsKingUnderAttack(king))
                {
                    king.Position = originalPosition;
                    return false;
                }

                // Restore original position
                king.Position = originalPosition;
            }

            return true;
        }

        private bool IsKingUnderAttack(ChessBasePiece king)
        {
            GetAllPieceMovements();

            // Check if any opponent piece can capture the king's position
            bool isUnderAttack = ChessPiecesMovements.Any(p => p.Movements.Contains(king.Position));

             return isUnderAttack;
        }




        public async Task RandomMovement()
        {
            var random = new Random();


            //Delay del bot al mover una ficha
            await Task.Delay(random.Next(1000, 5000));

            // Calculate possible movements
            GetAllPieceMovements();

            // Get pieces valid moves
            var playerPiecesMovements = ChessPiecesMovements
                .Where(p => p.Piece.Player1Color == Player1Turn && p.Movements.Any())
                .ToList();

            // If no valid moves available, return
            if (!playerPiecesMovements.Any())
            {
                return;
            }

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