using chess4connect.Mappers;
using chess4connect.Models.Database.DTOs;
using chess4connect.Models.Database.Entities;

namespace chess4connect.Services;

public class UserService
{
    private readonly UnitOfWork _unitOfWork;
    private readonly UserMapper _mapper;

    public UserService(UnitOfWork unitOfWork, UserMapper userMapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = userMapper;
    }

    public async Task<User> GetUserById(int id)
    {
        return await _unitOfWork.UserRepository.GetAllInfoById(id);

    }

    public async Task<List<UserAfterLoginDto>> GetUsers()
    {
        List<User> user = await _unitOfWork.UserRepository.GetAllUsers();

        return _mapper.ToDto(user).ToList() ;
    }

    public async Task<List<User>> GetAllFriends(int userId)
    {
        List<Friendship> acceptedFriedships = await _unitOfWork.FriendshipRepository.GetAllAcceptedFriendshipsByUserId(userId);

        List<User> friends = new List<User>();

        foreach (Friendship friendShip in acceptedFriedships)
        {
            int friendId = friendShip.UserId == userId ? friendShip.FriendId : friendShip.UserId;
            friends.Add (await _unitOfWork.UserRepository.GetUserById(friendId));
        }

        return friends;

    }
}
