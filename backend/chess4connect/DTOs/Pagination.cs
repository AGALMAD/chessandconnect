using chess4connect.Enums;

namespace chess4connect.DTOs
{
    public class Pagination
    {
        public int UserId { get; set; }
        public GameType GameType { get; set; }
        public int GamesCuantity { get; set; }
        public int ActualPage { get; set; }
    }
}

