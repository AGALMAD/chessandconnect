namespace chess4connect.Models.SocketComunication.Handlers.Services;

using chess4connect.Models.Database.Entities;
using chess4connect.Models.SocketComunication.MessageTypes;
using chess4connect.Services;


public class FriendMessageService
{


    private readonly UserService _userService;

    public FriendMessageService(UserService userService)
    {
        _userService = userService;
    }


    //public async Task<string> GetAllFriends(int userId, List<int> userConnected)
    //{
    //    List<FriendModel> friendsModel = new List<FriendModel>();

    //    List<User> friends = _userService.




    //}

}

