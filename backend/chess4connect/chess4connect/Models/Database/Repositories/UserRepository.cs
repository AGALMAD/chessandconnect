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

    public async Task<User> GetUserById(int userId)
    {
        return await GetQueryable()
            .Include(user => user.Plays)
            .Include(user => user.Friends)
            .FirstOrDefaultAsync(user => user.Id == userId);
    }

    public async Task<User> GetUserRequestsById(int userId)
    {
        return await GetQueryable()
        .Include(user => user.Requests.Where(request => request.FriendId == userId))
        .FirstOrDefaultAsync(user => user.Id == userId);
    }

    public async Task<List<User>> GetUsersByUserName(string nickName)
    {
        return await GetQueryable().Where(user => user.UserName == nickName).ToListAsync();
    }

    public async Task<User> GetAllInfoById(int id)
    {
        return await GetQueryable()
            .Include(user => user.Friends)
            .Include(user => user.Plays)
            //Faltan las peticiones de amistad
            .FirstOrDefaultAsync(user => user.Id == id);
    }

}
