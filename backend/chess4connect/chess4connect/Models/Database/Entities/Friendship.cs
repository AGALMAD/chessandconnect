using chess4connect.Enums;

namespace chess4connect.Models.Database.Entities
{
    public class Friendship
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int FriendId { get; set; }
        public FriendshipState State { get; set; }
        
    }
}
