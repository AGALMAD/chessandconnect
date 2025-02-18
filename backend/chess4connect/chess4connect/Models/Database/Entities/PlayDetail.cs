using chess4connect.Enums;

namespace chess4connect.Models.Database.Entities
{
    public class PlayDetail
    {
        public int Id { get; set; }

        public int PlayId { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

    }
}
