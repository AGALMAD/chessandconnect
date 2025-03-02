using chess4connect.Models.Database.Entities;
using chess4connect.Models.Database.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace chess4connect.Models.Database.Repositories
{
    public class PlayRepository : Repository<Play, int>
    {
        public PlayRepository(ChessAndConnectContext context) : base(context)
        {
        }

        public async Task<Play> GetPlaybyId(int id)
        {
            return await GetQueryable().Include(play => play.PlayDetails).FirstOrDefaultAsync(play => play.Id == id);
        }
    }
}
