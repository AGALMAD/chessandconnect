using chess4connect.Models.Database.Entities;
using chess4connect.Models.Database.Repositories.Base;

namespace chess4connect.Models.Database.Repositories
{
    public class PlayDetailRepository : Repository<PlayDetail, int>
    {
        public PlayDetailRepository(ChessAndConnectContext context) : base(context)
        {
        }
    }
}
