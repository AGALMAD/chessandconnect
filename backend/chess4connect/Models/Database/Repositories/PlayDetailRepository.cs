using chess4connect.Models.Database.Entities;
using chess4connect.Models.Database.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace chess4connect.Models.Database.Repositories
{
    public class PlayDetailRepository : Repository<PlayDetail, int>
    {
        public PlayDetailRepository(ChessAndConnectContext context) : base(context)
        {
        }

        public async Task<List<PlayDetail>> GetPlayDetailById(int userId)
        {
            return await GetQueryable()
                         .Where(playdetail => playdetail.UserId == userId)
                         .ToListAsync();
        }

    }
}
