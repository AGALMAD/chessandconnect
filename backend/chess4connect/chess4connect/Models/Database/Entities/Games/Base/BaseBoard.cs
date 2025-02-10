namespace chess4connect.Models.Database.Entities.Games.Base
{
    public class BaseBoard
    {
        public BasePiece[,] Board { get; set; }

        public BaseBoard(int rows, int columns)
        {
            Board = new BasePiece[rows, columns];
        }


    }
}
