using chess4connect.Enums;
using chess4connect.Models.Database.Entities;

namespace chess4connect.DTOs
{
    public class GameHistoryDto
    {
        public int Id { get; set; }

        public int TotalPages { get; set; }
        public List<GameHistoryDetailDto> Details { get; set; }
    }
}
