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
    public DbSet<PlayDetail> PlayDetails { get; set; }
    public DbSet<Friendship> Friendships { get; set; }



    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        #if DEBUG
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            optionsBuilder.UseSqlite($"DataSource={baseDir}{DATABASE_PATH}");
        #else
            optionsBuilder.UseMySql(_settings.DatabaseConnection, ServerVersion.AutoDetect(_settings.DatabaseConnection));
        #endif
    }

}
