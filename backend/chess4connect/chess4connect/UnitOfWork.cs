using chess4connect.Repositories;

namespace chess4connect;

public class UnitOfWork
{
    private readonly ChessAndConnectContext _context;

    private GameRepository _gameRepository;
    private PlayRepository _playRepository;
    private UserRepository _userRepository;
    private FriendshipRepository _friendshipRepository;


    public GameRepository OrderRepository => _gameRepository ??= new GameRepository(_context);
    public PlayRepository PlayRepository => _playRepository ??= new PlayRepository(_context);
    public UserRepository UserRepository => _userRepository ??= new UserRepository(_context);
    public FriendshipRepository FriendshipRepository => _friendshipRepository ??= new FriendshipRepository(_context);


    public UnitOfWork(ChessAndConnectContext context)
    {
        _context = context;
    }

    public ChessAndConnectContext Context => _context;

    public async Task<bool> SaveAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }

}
