using chess4connect.Models.Database.Entities;
using chess4connect.Services;
using Microsoft.AspNetCore.Mvc;

namespace chess4connect.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FriendshipController : ControllerBase
    {
        private FriendshipService _friendshipService;

        public FriendshipController(FriendshipService friendshipService)
        {
            _friendshipService = friendshipService;
        }

        [HttpPost ("user")]
        public async Task<User> getUser(string nickName)
        {
            User user = await _friendshipService.GetUserByNickName (nickName);
            return user;
        }
    }

}
