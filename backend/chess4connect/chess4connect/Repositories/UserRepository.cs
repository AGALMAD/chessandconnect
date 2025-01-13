using chess4connect.Models;
using chess4connect.Repositories.Base;

namespace chess4connect.Repositories
{
    public class UserRepository : Repository<User, int>
    {
        public UserRepository(ChessAndConnectContext context) : base(context)
        {
        }
    }
}
