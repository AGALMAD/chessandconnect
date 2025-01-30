using chess4connect.Enums;
using chess4connect.Mappers;
using chess4connect.Models.Database.DTOs;
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
    

    public UserService(UnitOfWork unitOfWork, UserMapper mapper, WebSocketNetwork webSocketNetwork)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _webSocketNetwork = webSocketNetwork;
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
        Friendship friendship = await _unitOfWork.FriendshipRepository.GetFriendshipByUsers(userId, friendId);

        if (friendship != null)
        {
            _unitOfWork.FriendshipRepository.Delete(friendship);
        }

        await _unitOfWork.SaveAsync();

    }

    public async Task GameInvitation(int userId, int friendId)
    {
        WebSocketHandler friendHandler = _webSocketNetwork.GetSocketByUserId(friendId);

        if (friendHandler != null)
        {
            var connectionMessage = new SocketMessage<GameInvitationModel>
            {
                Type = SocketCommunicationType.GAME_INVITATION,

                Data = new GameInvitationModel
                {
                    UserId = userId,
                    FriendId = friendId,
                    State = FriendshipState.Pending,

                }
            };

            string stringGamenInvitationMessage = JsonSerializer.Serialize(connectionMessage);

            await friendHandler.SendAsync(stringGamenInvitationMessage);


        }

    }
}
