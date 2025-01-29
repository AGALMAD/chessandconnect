using chess4connect.Models.Database.Entities;

namespace chess4connect.Services;

public class UserService
{
    private readonly UnitOfWork _unitOfWork;

    public UserService(UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<User> GetUserById(int id)
    {
        return await _unitOfWork.UserRepository.GetAllInfoById(id);

    }

    public async Task<List<User>> GetUsers()
    {
        return await _unitOfWork.UserRepository.GetAllUsers();
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
