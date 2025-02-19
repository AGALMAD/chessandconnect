using chess4connect.Models.Games.Base;
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

        public ChessPieceColor Turn {  get; set; }

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
            Board[0, 0] = new Rook(8, ChessPieceColor.BLACK, new Point(0, 0));
            Board[0, 1] = new Knight(9, ChessPieceColor.BLACK, new Point(0, 1));
            Board[0, 2] = new Bishop(10, ChessPieceColor.BLACK, new Point(0, 2));
            Board[0, 3] = new Queen(11, ChessPieceColor.BLACK, new Point(0, 3));
            Board[0, 4] = new King(12, ChessPieceColor.BLACK, new Point(0, 4));
            Board[0, 5] = new Bishop(13, ChessPieceColor.BLACK, new Point(0, 5));
            Board[0, 6] = new Knight(14, ChessPieceColor.BLACK, new Point(0, 6));
            Board[0, 7] = new Rook(15, ChessPieceColor.BLACK, new Point(0, 7));

            for (int i = 0; i < COLUMNS; i++)
            {
                Board[1, i] = new Pawn(16 + i, ChessPieceColor.BLACK, new Point(1, i));
            }


            //White pieces initialized in Board
            Board[7, 0] = new Rook(24, ChessPieceColor.WHITE, new Point(7, 0));
            Board[7, 1] = new Knight(25, ChessPieceColor.WHITE, new Point(7, 1));
            Board[7, 2] = new Bishop(26, ChessPieceColor.WHITE, new Point(7, 2));
            Board[7, 3] = new Queen(27, ChessPieceColor.WHITE, new Point(7, 3));
            Board[7, 4] = new King(28, ChessPieceColor.WHITE, new Point(7, 4));
            Board[7, 5] = new Bishop(29, ChessPieceColor.WHITE, new Point(7, 5));
            Board[7, 6] = new Knight(30, ChessPieceColor.WHITE, new Point(7, 6));
            Board[7, 7] = new Rook(31, ChessPieceColor.WHITE, new Point(7, 7));

            for (int i = 0; i < COLUMNS; i++)
            {
                Board[6, i] = new Pawn(32 + i, ChessPieceColor.WHITE, new Point(6, i));
            }


        }


        public void GetAllPieceMovements()
        {
            ChessPiecesMovements = new List<ChessPiecesMovements>();
            
            foreach (ChessBasePiece piece in Board)
            {
                //only pieces positions from actual player will be recalculated
                if (piece == null || piece.Color != Turn) continue;

                List<Point> movementList = new List<Point>();

                foreach (Point position in piece.BasicMovements)
                {
                    //calculate position adding the basic movement to piece position
                    Point nextMove = new Point
                    {
                        X = piece.Position.X + position.X,
                        Y = piece.Position.Y + position.Y
                    };

                    //making sure the next move will be inside the board
                    if (nextMove.X >= 0 && nextMove.X < 8 && nextMove.Y >= 0 && nextMove.Y < 8)
                    {
                        //making sure there is no piece in this cell
                        if (Board[nextMove.X, nextMove.Y] == null)
                        {
                            movementList.Add(nextMove);
                        }
                        //checking if this cell contains a opponent piece
                        else if (Board[nextMove.X, nextMove.Y].Color != piece.Color)
                        {
                            movementList.Add(nextMove);
                        }
                    }
                }
                //creating a new object of a piece and its movements
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
            return;
        }


        public int MovePiece(ChessMoveRequest moveRequest)
        {

            //Busca la pieza en la lista de piezas del tablero
            var piece = convertBoardToList().FirstOrDefault(p => p.Id == moveRequest.PieceId);

            if (piece != null)
            {
                //Verifica si el movimiento que quiere hacer es correcto
                var chessPieceMovements = ChessPiecesMovements.Where(p => p.Piece.Id == piece.Id).FirstOrDefault();

                if (chessPieceMovements != null && chessPieceMovements.Movements.Contains(new Point(moveRequest.MovementX, moveRequest.MovementY)))
                {
                    //Si el peon llega al final se convierte en reina
                    if ((piece.PieceType == PieceType.PAWN && piece.Color == ChessPieceColor.WHITE && moveRequest.MovementX == 0) ||
                        (piece.PieceType == PieceType.PAWN && piece.Color == ChessPieceColor.BLACK && moveRequest.MovementX == ROWS - 1))
                        piece = new Queen(piece.Id, piece.Color, piece.Position);



                    //Si el peón tiene una ficha delante no se puede mover
                    if (piece.PieceType == PieceType.PAWN)
                    {
                        //Movimiento hacia deltante
                        if(piece.Position.Y == moveRequest.MovementY)
                        {
                            //Si hay una pieza delante no se puede mover
                            if (Board[moveRequest.MovementX, moveRequest.MovementY] != null)
                                return -1;
                        }

                        // Movimiento diagonal 
                        if (Math.Abs(piece.Position.Y - moveRequest.MovementY) == 1 &&
                            moveRequest.MovementX == piece.Position.X + (piece.Color == ChessPieceColor.WHITE ? -1 : 1))
                        {
                            // Si la casilla está vacía o tiene una pieza del mismo color, no es un movimiento válido
                            if (Board[moveRequest.MovementX, moveRequest.MovementY] == null ||
                                Board[moveRequest.MovementX, moveRequest.MovementY].Color == piece.Color)
                            {
                                return -1;
                            }
                        }

                    }

                    //Comprueba Jaque Mate
                    if (Board[moveRequest.MovementX, moveRequest.MovementY] != null && Board[moveRequest.MovementX, moveRequest.MovementY].PieceType == PieceType.KING && Board[moveRequest.MovementX, moveRequest.MovementY].Color != Turn)
                    {
                        return 1;
                    }



                    //Mueve la pieza y actualiza su posición
                    Board[piece.Position.X, piece.Position.Y] = null;

                    Board[moveRequest.MovementX, moveRequest.MovementY] = piece;
                    piece.Position = new Point(moveRequest.MovementX, moveRequest.MovementY);

                    //Resta el tiempo en segundos que ha tardado en mover
                    TimeSpan remaninder = DateTime.Now.Subtract(StartTurnDateTime);

                    if (piece.Color == ChessPieceColor.WHITE)
                        Player1Time -= remaninder;
                    else
                        Player2Time -= remaninder;
                    


                    //Cambia el turno
                    Turn = Turn == ChessPieceColor.BLACK ? ChessPieceColor.WHITE : ChessPieceColor.BLACK;

                    //Fecha de inicio del turno
                    StartTurnDateTime = DateTime.Now;

                    return 0;
                }

            }

            return -1;
        }



        public void RandomMovement()
        {
            

            // Calcula los posibles movimientos que puede hacer 
            GetAllPieceMovements();

            var playerPiecesMovements = ChessPiecesMovements
                .Where(p => p.Piece.Color == Turn)
                .ToList();

            
            if (playerPiecesMovements.Count == 0)
            {
                Console.WriteLine("No pieces to move.");
                return;
            }

            var random = new Random();
            ChessPiecesMovements selectedPieceMovement;
            Point randomMove;

            do
            {
                
                selectedPieceMovement = playerPiecesMovements[random.Next(playerPiecesMovements.Count)];
                randomMove = selectedPieceMovement.Movements[random.Next(selectedPieceMovement.Movements.Count)];



            } while (MovePiece(new ChessMoveRequest
            {
                PieceId = selectedPieceMovement.Piece.Id,
                MovementX = randomMove.X,
                MovementY = randomMove.Y,
            }) != 0);



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