using chess4connect.Models.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace chess4connect;

public class ChessAndConnectContext : DbContext
{
    private const string DATABASE_PATH = "chessAndConnect.db";

    private readonly Settings _settings;
    public ChessAndConnectContext(Settings settings)
    {
        _settings = settings;
    }


    public DbSet<User> Users { get; set; }
    public DbSet<Play> Plays { get; set; }
    public DbSet<Game> Games { get; set; }
    public DbSet<Friendship> Friendships { get; set; }



    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite(_settings.DatabaseConnection);
    }
}
