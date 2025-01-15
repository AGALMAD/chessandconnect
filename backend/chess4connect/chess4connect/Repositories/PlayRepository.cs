using chess4connect.Repositories.Base;

namespace chess4connect.Repositories
{
    public class PlayRepository : Repository<PlayRepository, int>
    {
        public PlayRepository(ChessAndConnectContext context) : base(context)
        {
        }
    }
}
