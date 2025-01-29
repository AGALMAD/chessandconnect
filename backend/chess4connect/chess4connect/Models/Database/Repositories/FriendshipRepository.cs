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

        public async Task<List<Friendship>> GetAllFriendshipFromUser (int userId)
        {
            return await GetQueryable().Where(friendship => friendship.FriendId == userId).ToListAsync();
        }


        public async Task<List<Friendship>> GetAllPendingFriendshipsByUserId(int userId)
        {
            return await GetQueryable().
                Where(friendship => (friendship.UserId == userId || friendship.FriendId == userId) && friendship.State == Enums.FriendshipState.Pending)
                .ToListAsync();
        }

        public async Task<List<Friendship>> GetAllAcceptedFriendshipsByUserId(int userId)
        {
            return await GetQueryable().
                Where(friendship => (friendship.UserId == userId || friendship.FriendId == userId) && friendship.State == Enums.FriendshipState.Accepted)
                .ToListAsync();
        }





    }
}
