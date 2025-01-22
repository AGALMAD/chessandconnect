using chess4connect.Models.Database.Entities;
using chess4connect.Models.Database.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace chess4connect.Models.Database.Repositories;

public class UserRepository : Repository<User, int>
{
    public UserRepository(ChessAndConnectContext context) : base(context) { }


    public async Task<User> GetUserByCredential(string credential)
    {
        return await GetQueryable()
            .Include(user => user.Plays)
            .FirstOrDefaultAsync(user => user.Email == credential || user.UserName == credential);
    }
    public async Task<User> GetUserByUserName(string nickName)
    {
        return await GetQueryable().FirstAsync(user => user.UserName == nickName);
    }
}
