namespace chess4connect.Models.Database.Entities
{
    public class Piece
    {
        public string Color { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }

        public Piece(string color, int row, int col)
        {
            Color = color;
            Row = row;
            Column = col;
        }
    }
}
