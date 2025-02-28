using chess4connect.DTOs;
using chess4connect.Enums;
using chess4connect.Mappers;
using chess4connect.Models.Database.Entities;
using chess4connect.Models.SocketComunication.Handlers;
using chess4connect.Models.SocketComunication.MessageTypes;
using System.Text.Json;

namespace chess4connect.Services;

public class UserService
{
    private readonly UnitOfWork _unitOfWork;
    private readonly UserMapper _mapper;
    private readonly WebSocketNetwork _webSocketNetwork;
    private readonly PasswordService _passwordService;
    private readonly ImageService _imageService;
    

    public UserService(UnitOfWork unitOfWork, UserMapper mapper, WebSocketNetwork webSocketNetwork, PasswordService passwordService,
         ImageService imageService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _webSocketNetwork = webSocketNetwork;
        _passwordService = passwordService;
        _imageService = imageService;
    }

    public async Task<User> GetUserById(int id)
    {
        return await _unitOfWork.UserRepository.GetAllInfoById(id);

    }

    public async Task<List<UserAfterLoginDto>> GetUsers(int id)
    {
        List<User> user = await _unitOfWork.UserRepository.GetAllUsers(id);

        return _mapper.ToDto(user).ToList() ;
    }

    public async Task<UserAfterLoginDto> GetUser(int id)
    {
        User user = await _unitOfWork.UserRepository.GetUserById(id);

        return _mapper.ToDto(user);
    }

    public async Task UpdateUser(UserSignUpDto user, int id)
    {
        UserAfterLoginDto userToUpdate = await GetUser(id);

        userToUpdate.UserName = user.UserName;
        userToUpdate.Email = user.Email;

        _unitOfWork.UserRepository.Update(_mapper.ToEntity(userToUpdate));
        await _unitOfWork.SaveAsync();

    }

        public async Task UpdateAvatar(IFormFile ImagePath, int id)
        {
            UserAfterLoginDto userToUpdate = await GetUser(id);

            userToUpdate.AvatarImageUrl = await _imageService.InsertAsync(ImagePath);

            _unitOfWork.UserRepository.Update(_mapper.ToEntity(userToUpdate));
            await _unitOfWork.SaveAsync();
    }

    public async Task UpdateUserPassword(int id,string password)
    {
        User userToUpdate = await GetUserById(id);

        userToUpdate.Password = _passwordService.Hash(password);

        _unitOfWork.UserRepository.Update(userToUpdate);
        await _unitOfWork.SaveAsync();
    }

    public async Task<List<FriendModel>> GetAllFriendsWithState(int userId)
    {
        List<Friendship> acceptedFriedships = await _unitOfWork.FriendshipRepository.GetAllAcceptedFriendshipsByUserId(userId);

        List<FriendModel> friendsWithState = new List<FriendModel>();

        List<int> usersConnected = _webSocketNetwork.GetAllUserIds();

        foreach (Friendship friendShip in acceptedFriedships)
        {
            int friendId = friendShip.UserId == userId ? friendShip.FriendId : friendShip.UserId;
            User completeFriend = await _unitOfWork.UserRepository.GetUserById(friendId);

            friendsWithState.Add(new FriendModel
            {
                Id = completeFriend.Id,
                UserName = completeFriend.UserName,
                AvatarImageUrl = completeFriend.AvatarImageUrl,
                Connected = usersConnected.Contains(friendId),
            });

        }


        return friendsWithState;

    }


    public async Task DeleteFriend(int userId, int friendId)
    {
        Friendship friendship = await _unitOfWork.FriendshipRepository.GetFriendByUser(userId, friendId);

        if (friendship != null)
        {
            _unitOfWork.FriendshipRepository.Delete(friendship);
        }

        await _unitOfWork.SaveAsync();

    }

   
}
