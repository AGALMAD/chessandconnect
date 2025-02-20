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
            string connection = "Server=db14304.databaseasp.net; Database=db14304; Uid=db14304; Pwd=pT@45eW!S?d8;";
            optionsBuilder.UseMySql(connection, ServerVersion.AutoDetect(connection));
#endif
    }

}
