using chess4connect.Models.Database.Repositories.Base;

namespace chess4connect.Models.Database.Repositories
{
    public class PlayRepository : Repository<PlayRepository, int>
    {
        public PlayRepository(ChessAndConnectContext context) : base(context)
        {
        }
    }
}
