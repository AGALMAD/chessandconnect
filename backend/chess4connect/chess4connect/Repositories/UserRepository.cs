using chess4connect.Models;
using chess4connect.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace chess4connect.Repositories;

public class UserRepository : Repository<User, int>
{
    public UserRepository(ChessAndConnectContext context) : base(context){}

    public async Task<User> GetUserById(int id)
    {
        return await GetQueryable()
            .Include(user => user.Plays)
            .FirstOrDefaultAsync(user => user.Id == id);
    }
}
