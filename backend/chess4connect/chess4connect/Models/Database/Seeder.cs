using chess4connect.Models.Database.Entities;
using chess4connect.Services;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

namespace chess4connect.Models.Database;

public class Seeder
{
    private readonly ChessAndConnectContext _chessAndConnectContext;
    private readonly PasswordService _passwordService;

    public Seeder(ChessAndConnectContext chessAndConnectContext, PasswordService passwordService)
    {
        _chessAndConnectContext = chessAndConnectContext;
        _passwordService = passwordService;
    }

    public async Task SeedAsync()
    {
        await SeedUsersAsync();
        await _chessAndConnectContext.SaveChangesAsync();
    }

    private async Task SeedUsersAsync()
    {
        User[] users =
        [
            new User(){
                UserName = "admin",
                Email = "admin@gmail.com",
                Password = _passwordService.Hash("admin"),
                Role = "admin",
                AvatarImageUrl = "",
                Banned = false,

            }
        ];

        await _chessAndConnectContext.Users.AddRangeAsync(users);
    }


}
