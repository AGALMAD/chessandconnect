using chess4connect.Enums;

namespace chess4connect.Models.SocketComunication.MessageTypes
{
    public class FriendshipModel
    {
        public FriendshipState State { get; set; }
        public int UserId { get; set; }
        public int FriendId { get; set; }
    }
}
