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
    }

}
