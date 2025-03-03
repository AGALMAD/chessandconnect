using chess4connect.Models.Games.Chess.Chess.Pieces;
using chess4connect.Models.Games.Chess.Chess.Pieces.Base;
using chess4connect.Models.Games.Chess.Chess.Pieces.Types;
using chess4connect.Services;
using System.Drawing;
using System.IO.Pipelines;

namespace chess4connect.Models.Games.Chess.Chess
{
    public class ChessBoard
    {
        public static int ROWS = 8;
        public static int COLUMNS = 8;
        public GameTimer remainingTime;
        public List<ChessPiecesMovements> ChessPiecesMovements { get; set; }

        private ChessBasePiece[,] Board = new ChessBasePiece[ROWS, COLUMNS];

        public event Action OnTimeExpired;
        public bool Player1Turn { get; set; } = true;

        public delegate void TimeExpiredEventHandler(bool isPlayer1Turn);

        private System.Timers.Timer _timer;

        public event Action<bool> TimeExpired;
        //Tiempo en segundo de cada turno
        public TimeSpan Player1Time { get; set; } = TimeSpan.FromSeconds(60);
        public TimeSpan Player2Time { get; set; } = TimeSpan.FromSeconds(60);

        //Fecha de inicio de cada turno
        public DateTime StartTurnDateTime { get; set; }

        protected virtual void OnTimeExpiredEvent()
        {
            OnTimeExpired?.Invoke();
        }

        public ChessBoard()
        {
            PlacePiecesInBoard();
            remainingTime = new GameTimer();
            remainingTime.OnTimeExpired += CheckTimeExpired; // Suscribimos el evento del Timer
        }


        private void PlacePiecesInBoard()
        {
            Board = new ChessBasePiece[ROWS, COLUMNS];

            //Black pieces initialized in Board
            Board[0, 0] = new Rook(8, false, new Point(0, 0)) { HasMoved = false };
            Board[0, 1] = new Knight(9, false, new Point(0, 1)) { HasMoved = false };
            Board[0, 2] = new Bishop(10, false, new Point(0, 2)) { HasMoved = false };
            Board[0, 3] = new Queen(11, false, new Point(0, 3)) { HasMoved = false };
            Board[0, 4] = new King(12, false, new Point(0, 4)) { HasMoved = false };
            Board[0, 5] = new Bishop(13, false, new Point(0, 5)) { HasMoved = false };
            Board[0, 6] = new Knight(14, false, new Point(0, 6)) { HasMoved = false };
            Board[0, 7] = new Rook(15, false, new Point(0, 7)) { HasMoved = false };

            for (int i = 0; i < COLUMNS; i++)
            {
                Board[1, i] = new Pawn(16 + i, false, new Point(1, i)) { HasMoved = false };
            }

            //White pieces initialized in Board
            Board[7, 0] = new Rook(24, true, new Point(7, 0)) { HasMoved = false };
            Board[7, 1] = new Knight(25, true, new Point(7, 1)) { HasMoved = false };
            Board[7, 2] = new Bishop(26, true, new Point(7, 2)) { HasMoved = false };
            Board[7, 3] = new Queen(27, true, new Point(7, 3)) { HasMoved = false };
            Board[7, 4] = new King(28, true, new Point(7, 4)) { HasMoved = false };
            Board[7, 5] = new Bishop(29, true, new Point(7, 5)) { HasMoved = false };
            Board[7, 6] = new Knight(30, true, new Point(7, 6)) { HasMoved = false };
            Board[7, 7] = new Rook(31, true, new Point(7, 7)) { HasMoved = false };

            for (int i = 0; i < COLUMNS; i++)
            {
                Board[6, i] = new Pawn(32 + i, true, new Point(6, i)) { HasMoved = false };
            }

            remainingTime = new GameTimer();

            //timmer
            _timer = new System.Timers.Timer(1000); 
            _timer.AutoReset = true;
            _timer.Enabled = true;
        }
        private void OnTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            CheckTimeExpired(); 
        }

        public void GetAllPieceMovements()
        {
            if (Player1Turn)
            {
                remainingTime.StartTimer(Player1Time);
                Console.WriteLine(Player1Time);
            }
            else
            {
                remainingTime.StartTimer(Player2Time);
            }


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
            // Movimientos normales del rey
            foreach (Point move in piece.BasicMovements)
            {
                int newX = piece.Position.X + move.X;
                int newY = piece.Position.Y + move.Y;

                if (newX >= 0 && newX < 8 && newY >= 0 && newY < 8)
                {
                    if (Board[newX, newY] == null || Board[newX, newY].Player1Piece != piece.Player1Piece)
                    {
                        // Verificar si el movimiento pondría al rey en jaque
                        var originalPos = piece.Position;
                        var capturedPiece = Board[newX, newY];

                        // Simular el movimiento en el tablero
                        Board[originalPos.X, originalPos.Y] = null;
                        Board[newX, newY] = piece;
                        piece.Position = new Point(newX, newY);

                        // Verificar si el rey estaría en jaque después del movimiento
                        bool kingInCheck = IsSquareUnderAttack(newX, newY, piece.Player1Piece);

                        // Restaurar el tablero
                        Board[newX, newY] = capturedPiece;
                        Board[originalPos.X, originalPos.Y] = piece;
                        piece.Position = originalPos;

                        // Solo añadir el movimiento si no pone al rey en jaque
                        if (!kingInCheck)
                        {
                            movementList.Add(new Point(newX, newY));
                        }
                    }
                }
            }

            // Verificar enroque (solo si el rey no ha movido)
            if (!piece.HasMoved && !IsKingUnderAttack(piece.Player1Piece))
            {
                CheckCastling(piece, movementList);
            }
        }

        private void CheckCastling(ChessBasePiece king, List<Point> movementList)
        {
            int row = king.Position.X;
            int col = king.Position.Y;

            // Enroque corto (hacia la derecha, columna 7)
            if (Board[row, 7] is Rook rightRook && !rightRook.HasMoved && rightRook.Player1Piece == king.Player1Piece)
            {
                bool pathClear = true;
                // Verificar que no hay piezas entre el rey y la torre
                for (int y = col + 1; y < 7; y++)
                {
                    if (Board[row, y] != null)
                    {
                        pathClear = false;
                        break;
                    }
                }

                // Verificar que el rey no pasa por jaque
                bool kingPassesThroughCheck = false;
                if (pathClear)
                {
                    // Comprobar las casillas intermedias
                    if (IsSquareUnderAttack(row, col + 1, king.Player1Piece) ||
                        IsSquareUnderAttack(row, col + 2, king.Player1Piece))
                    {
                        kingPassesThroughCheck = true;
                    }
                }

                if (pathClear && !kingPassesThroughCheck)
                {
                    // Marcar el movimiento de enroque (dos a la derecha)
                    movementList.Add(new Point(row, col + 2));
                }
            }

            // Enroque largo (hacia la izquierda, columna 0)
            if (Board[row, 0] is Rook leftRook && !leftRook.HasMoved && leftRook.Player1Piece == king.Player1Piece)
            {
                bool pathClear = true;
                // Verificar que no hay piezas entre el rey y la torre
                for (int y = col - 1; y > 0; y--)
                {
                    if (Board[row, y] != null)
                    {
                        pathClear = false;
                        break;
                    }
                }

                // Verificar que el rey no pasa por jaque
                bool kingPassesThroughCheck = false;
                if (pathClear)
                {
                    // Comprobar las casillas intermedias
                    if (IsSquareUnderAttack(row, col - 1, king.Player1Piece) ||
                        IsSquareUnderAttack(row, col - 2, king.Player1Piece))
                    {
                        kingPassesThroughCheck = true;
                    }
                }

                if (pathClear && !kingPassesThroughCheck)
                {
                    // Marcar el movimiento de enroque (dos a la izquierda)
                    movementList.Add(new Point(row, col - 2));
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

        public void CheckTimeExpired()
        {
            _timer.Stop();
            OnTimeExpiredEvent();
        }


        public int MovePiece(ChessMoveRequest moveRequest)
        {
            

            // Find the piece
            var piece = convertBoardToList().FirstOrDefault(p => p.Id == moveRequest.PieceId);
            if (piece == null) return -1;

            // Verify if there's valid movement
            var chessPieceMovements = ChessPiecesMovements.FirstOrDefault(p => p.Piece.Id == piece.Id);
            if (chessPieceMovements == null ||
                !chessPieceMovements.Movements.Contains(new Point(moveRequest.MovementX, moveRequest.MovementY)))
            {
                return -1;
            }

            // Store the old position
            Point oldPosition = piece.Position;

            // Check for castling (king moving 2 squares horizontally)
            bool isCastling = false;
            if (piece.PieceType == PieceType.KING && Math.Abs(moveRequest.MovementY - oldPosition.Y) == 2)
            {
                isCastling = true;
            }

            // Execute the move
            Board[oldPosition.X, oldPosition.Y] = null;

            // Pawn promotion
            if (piece.PieceType == PieceType.PAWN &&
                ((piece.Player1Piece && moveRequest.MovementX == 0) ||
                 (!piece.Player1Piece && moveRequest.MovementX == ROWS - 1)))
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

                // Handle castling movement (move the rook too)
                if (isCastling)
                {
                    int rookY;
                    int newRookY;

                    //short castling
                    if (moveRequest.MovementY > oldPosition.Y)
                    {
                        rookY = 7; // Posición inicial de la torre del lado corto
                        newRookY = moveRequest.MovementY - 1;
                    }
                    else // long castling
                    {
                        rookY = 0;
                        newRookY = moveRequest.MovementY + 1;
                    }

                    // Mover la torre
                    var rook = Board[oldPosition.X, rookY];
                    Board[oldPosition.X, rookY] = null;
                    Board[oldPosition.X, newRookY] = rook;
                    rook.Position = new Point(oldPosition.X, newRookY);
                    rook.HasMoved = true;
                }

                // Update HasMoved flag
                piece.HasMoved = true;
            }

            // Update time
            TimeSpan timeSpent = DateTime.Now.Subtract(StartTurnDateTime);
            if (piece.Player1Piece)
                Player1Time -= timeSpent;
            else
                Player2Time -= timeSpent;


            StartTurnDateTime = DateTime.Now;


            // Check if the move results in checkmate
            if (IsCheckmate())
            {
                return 1; // Checkmate
            }

            // Change turn
            Player1Turn = !Player1Turn;
            StartTurnDateTime = DateTime.Now;
            Console.WriteLine($"Tiempo inicial Jugador 1: {Player1Time.TotalSeconds}");
            Console.WriteLine($"Tiempo inicial Jugador 2: {Player2Time.TotalSeconds}");


            // Recalculate all possible moves for the new position
            GetAllPieceMovements();

            return 0;
        }



        // check for checkmate
        private bool IsCheckmate()
        {

            bool opponentColor = !Player1Turn;

            // checking check
            if (!IsKingUnderAttack(opponentColor))
                return false;

            // opponent pieces
            var opponentPieces = convertBoardToList().Where(p => p.Player1Piece == opponentColor).ToList();

            // checking if some piece can block check
            foreach (var piece in opponentPieces)
            {
                List<Point> possibleMoves = new List<Point>();

                switch (piece.PieceType)
                {
                    case PieceType.PAWN:
                        CalculatePawnMovesForCheck(piece, possibleMoves);
                        break;
                    case PieceType.KNIGHT:
                        CalculateKnightMovesForCheck(piece, possibleMoves);
                        break;
                    case PieceType.KING:
                        CalculateKingMovesForCheck(piece, possibleMoves);
                        break;
                    case PieceType.ROOK:
                    case PieceType.BISHOP:
                    case PieceType.QUEEN:
                        CalculateSlidingMovesForCheck(piece, possibleMoves);
                        break;
                }

                foreach (var move in possibleMoves)
                {
                    // save original position
                    var originalPiecePosition = piece.Position;
                    var targetPiece = Board[move.X, move.Y];

                    // simulate movement
                    Board[originalPiecePosition.X, originalPiecePosition.Y] = null;
                    Board[move.X, move.Y] = piece;
                    piece.Position = move;

                    // checking if after moving king still in check
                    bool kingStillInCheck = IsKingUnderAttack(opponentColor);


                    // restore board
                    Board[move.X, move.Y] = targetPiece;
                    Board[originalPiecePosition.X, originalPiecePosition.Y] = piece;
                    piece.Position = originalPiecePosition;

                    // if there is possible movement, invalid checkmate
                    if (!kingStillInCheck)
                        return false;
                }
            }

            // if there is no possible movement, checkmate true
            return true;
        }


        private void CalculatePawnMovesForCheck(ChessBasePiece piece, List<Point> movementList)
        {
            foreach (Point move in piece.BasicMovements)
            {
                int newX = piece.Position.X + move.X;
                int newY = piece.Position.Y + move.Y;


                if (newX < 0 || newX >= 8 || newY < 0 || newY >= 8)
                    continue;


                if (move.Y == 0)
                {

                    if (Board[newX, newY] != null)
                        continue;


                    if (Math.Abs(move.X) == 2)
                    {
                        int direction = piece.Player1Piece ? -1 : 1;
                        int intermediateX = piece.Position.X + direction;

                        if (Board[intermediateX, newY] != null)
                            continue;
                    }

                    movementList.Add(new Point(newX, newY));
                }

                else if (Board[newX, newY]?.Player1Piece != piece.Player1Piece && Board[newX, newY] != null)
                {
                    movementList.Add(new Point(newX, newY));
                }
            }
        }

        private void CalculateKnightMovesForCheck(ChessBasePiece piece, List<Point> movementList)
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

        private void CalculateKingMovesForCheck(ChessBasePiece piece, List<Point> movementList)
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

        private void CalculateSlidingMovesForCheck(ChessBasePiece piece, List<Point> movementList)
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

        // king position
        private (int, int) FindKingPosition(bool isPlayer1)
        {
            for (int x = 0; x < ROWS; x++)
            {
                for (int y = 0; y < COLUMNS; y++)
                {
                    var piece = Board[x, y];
                    if (piece != null && piece.PieceType == PieceType.KING && piece.Player1Piece == isPlayer1)
                    {
                        return (x, y);
                    }
                }
            }
            throw new Exception($"Rey no encontrado para el jugador {(isPlayer1 ? "blanco" : "negro")}. Revisa el tablero.");
        }


        private bool IsSquareUnderAttack(int row, int col, bool isPlayerSquare)
        {

            int pawnDirection = isPlayerSquare ? 1 : -1;
            if (row + pawnDirection >= 0 && row + pawnDirection < ROWS)
            {

                if (col - 1 >= 0 &&
                    Board[row + pawnDirection, col - 1] is Pawn &&
                    Board[row + pawnDirection, col - 1].Player1Piece != isPlayerSquare)
                    return true;

                if (col + 1 < COLUMNS &&
                    Board[row + pawnDirection, col + 1] is Pawn &&
                    Board[row + pawnDirection, col + 1].Player1Piece != isPlayerSquare)
                    return true;
            }


            int[,] knightMoves = { { -2, -1 }, { -2, 1 }, { -1, -2 }, { -1, 2 }, { 1, -2 }, { 1, 2 }, { 2, -1 }, { 2, 1 } };
            for (int i = 0; i < 8; i++)
            {
                int newRow = row + knightMoves[i, 0];
                int newCol = col + knightMoves[i, 1];

                if (newRow >= 0 && newRow < ROWS && newCol >= 0 && newCol < COLUMNS &&
                    Board[newRow, newCol] is Knight &&
                    Board[newRow, newCol].Player1Piece != isPlayerSquare)
                    return true;
            }


            int[,] directions = { { -1, 0 }, { 1, 0 }, { 0, -1 }, { 0, 1 }, { -1, -1 }, { -1, 1 }, { 1, -1 }, { 1, 1 } };
            for (int i = 0; i < 8; i++)
            {
                int dx = directions[i, 0];
                int dy = directions[i, 1];


                for (int distance = 1; distance < 8; distance++)
                {
                    int newRow = row + dx * distance;
                    int newCol = col + dy * distance;

                    if (newRow < 0 || newRow >= ROWS || newCol < 0 || newCol >= COLUMNS)
                        break;

                    if (Board[newRow, newCol] == null)
                        continue;

                    if (Board[newRow, newCol].Player1Piece == isPlayerSquare)
                        break;

                    bool isRook = Board[newRow, newCol] is Rook;
                    bool isBishop = Board[newRow, newCol] is Bishop;
                    bool isQueen = Board[newRow, newCol] is Queen;

                    if ((isRook && i < 4) || (isBishop && i >= 4) || isQueen)
                        return true;

                    break;
                }
            }


            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    if (dx == 0 && dy == 0) continue;

                    int newRow = row + dx;
                    int newCol = col + dy;

                    if (newRow >= 0 && newRow < ROWS && newCol >= 0 && newCol < COLUMNS &&
                        Board[newRow, newCol] is King &&
                        Board[newRow, newCol].Player1Piece != isPlayerSquare)
                        return true;
                }
            }

            return false;
        }


        public bool IsKingUnderAttack(bool isPlayer1)
        {
            (int kingRow, int kingCol) = FindKingPosition(isPlayer1);

            return IsSquareUnderAttack(kingRow, kingCol, isPlayer1);
        }


        public async Task<bool> RandomMovement()
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
                return false;
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
                        return IsCheckmate();
                    }

                    attempts++;
                }
                catch (Exception)
                {
                    attempts++;
                }
            }

            return false;
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