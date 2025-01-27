using chess4connect.Enums;
using chess4connect.Models.Database.Entities;
using chess4connect.Models.Database.Repositories.Base;

namespace chess4connect.Models.Database.Repositories
{
    public class GameRepository : Repository<Play, int>
    {
        public GameRepository(ChessAndConnectContext context) : base(context)
        {
        }
    }
}
