using chess4connect.Models.Database.Entities;
using chess4connect.Models.Database.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace chess4connect.Models.Database.Repositories
{
    public class FriendshipRepository : Repository<Friendship, int>
    {
        public FriendshipRepository(ChessAndConnectContext context) : base(context)
        {
        }

        public async Task<Friendship> GetFriendshipByUsers (int userId, int friendId)
        {
            return await GetQueryable().FirstAsync(friendship => friendship.UserId == userId && friendship.FriendId == friendId);
        }
    }
}
