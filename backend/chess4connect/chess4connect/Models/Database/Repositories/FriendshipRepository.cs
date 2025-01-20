using chess4connect.Models.Database.Entities;
using chess4connect.Models.Database.Repositories.Base;

namespace chess4connect.Models.Database.Repositories
{
    public class FriendshipRepository : Repository<Friendship, int>
    {
        public FriendshipRepository(ChessAndConnectContext context) : base(context)
        {
        }
    }
}
