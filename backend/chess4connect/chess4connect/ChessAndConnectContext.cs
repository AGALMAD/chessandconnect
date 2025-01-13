using chess4connect.Models;
using Microsoft.EntityFrameworkCore;

namespace chess4connect;

public class ChessAndConnectContext : DbContext
{
    private readonly Settings _settings;
    public ChessAndConnectContext(Settings settings)
    {
        _settings = settings;
    }

    private const string DATABASE_PATH = "chessAndConnect.db";


    public DbSet<User> Users { get; set; }
    public DbSet<Play> Plays { get; set; }
    public DbSet<Game> Games { get; set; }


}
