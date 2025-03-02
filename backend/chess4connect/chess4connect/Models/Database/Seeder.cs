using chess4connect.Enums;
using chess4connect.Models.Database.Entities;
using chess4connect.Services;
using Microsoft.EntityFrameworkCore;
using System.Numerics;
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
        await SeedFriendshipsAsync();
        await _chessAndConnectContext.SaveChangesAsync();
        await SeedPlayDetailsAsync();
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
                AvatarImageUrl = "UserProfilePicture/perfil_por_defecto.png",
                Banned = false,

            },
            new User(){
                UserName = "ale",
                Email = "ale@gmail.com",
                Password = _passwordService.Hash("ale"),
                Role = "user",
                AvatarImageUrl = "UserProfilePicture/perfil_por_defecto.png",
                Banned = false,

            },
            new User(){
                UserName = "noe",
                Email = "noe@gmail.com",
                Password = _passwordService.Hash("noe"),
                Role = "user",
                AvatarImageUrl = "UserProfilePicture/perfil_por_defecto.png",
                Banned = false,

            },
            new User(){
                UserName = "manu",
                Email = "manu@gmail.com",
                Password = _passwordService.Hash("manu"),
                Role = "user",
                AvatarImageUrl = "UserProfilePicture/perfil_por_defecto.png",
                Banned = false,

            },

        ];

        await _chessAndConnectContext.Users.AddRangeAsync(users);


    }
    private async Task SeedFriendshipsAsync()
    {
        Friendship[] friendships = [
            new Friendship(){
                UserId = 2,
                FriendId = 3,
                State = Enums.FriendshipState.Accepted,
            },
            new Friendship(){
                UserId = 2,
                FriendId = 4,
                State = Enums.FriendshipState.Accepted,
            },
            new Friendship(){
                UserId = 3,
                FriendId = 4,
                State = Enums.FriendshipState.Accepted,
            },


        ];

        await _chessAndConnectContext.Friendships.AddRangeAsync(friendships);


    }

    private async Task SeedPlayDetailsAsync()
    {
        var playDetails1 = new List<PlayDetail>
        {
                new PlayDetail { PlayId = 1, UserId = 3, GameResult = GameResult.WIN },
                new PlayDetail { PlayId = 1, UserId = 4, GameResult = GameResult.LOSE }
            };

        var playDetails2 = new List<PlayDetail>
        {
                new PlayDetail { PlayId = 2, UserId = 3, GameResult = GameResult.LOSE },
                new PlayDetail { PlayId = 2, UserId = 4, GameResult = GameResult.WIN }
            };

        await _chessAndConnectContext.PlayDetails.AddRangeAsync(playDetails1);
        await _chessAndConnectContext.PlayDetails.AddRangeAsync(playDetails2);
    }





}
