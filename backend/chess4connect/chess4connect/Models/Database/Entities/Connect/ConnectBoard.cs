namespace chess4connect.Models.Database.Entities.Connect
{
    public class ConnectBoard
    {
        public int Rows { get; set; } = 6;
        public int Columns { get; set; } = 7;
        public List<Piece> Pieces { get; set; }
    }
}
