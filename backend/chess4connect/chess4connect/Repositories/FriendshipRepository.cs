using chess4connect.Models;
using chess4connect.Repositories.Base;

namespace chess4connect.Repositories
{
    public class FriendshipRepository : Repository<Friendship, int>
    {
        public FriendshipRepository(ChessAndConnectContext context) : base(context)
        { 
        }
    }
}
