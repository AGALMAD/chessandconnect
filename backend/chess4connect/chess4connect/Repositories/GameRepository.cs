using chess4connect.Models;
using chess4connect.Repositories.Base;

namespace chess4connect.Repositories
{
    public class GameRepository : Repository<Game, int>
    {
        public GameRepository(ChessAndConnectContext context) : base(context)
        {
        }
    }
}
