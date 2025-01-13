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

}
