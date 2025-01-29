using chess4connect.Enums;
using chess4connect.Mappers;
using chess4connect.Models.Database.DTOs;
using chess4connect.Models.Database.Entities;
using chess4connect.Models.SocketComunication.Handlers;
using chess4connect.Models.SocketComunication.MessageTypes;
using chess4connect.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;

namespace chess4connect.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FriendshipController : ControllerBase
    {
        private WebSocketNetwork _webSocketNetwork;
        private FriendshipService _friendshipService;
        private FriendMapper _friendMapper;
        private UnitOfWork _unitOfWork;

        public FriendshipController(FriendshipService friendshipService, FriendMapper friendMapper, UnitOfWork unitOfWork, WebSocketNetwork webSocketNetwork)
        {
            _webSocketNetwork = webSocketNetwork;
            _friendshipService = friendshipService;
            _friendMapper = friendMapper;
            _unitOfWork = unitOfWork;
        }

        [Authorize]
        [HttpGet ("getusers")]
        public async Task<IEnumerable<FriendDto>> GetUsersByNickname(string nickName)
        {
            IEnumerable<User> users = await _friendshipService.GetAllUsersByNickname(nickName);

            IEnumerable<FriendDto> usersDtos = _friendMapper.ToDto(users);

            return usersDtos;
        }

        [Authorize]
        [HttpGet("getfriends")]
        public async Task<ActionResult<IEnumerable<FriendDto>>> GetAllFriends()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out var userIdInt))
            {
                return Unauthorized("El usuario no está autenticado.");
            }

            List<User> friends = await _friendshipService.GetAllUserFriends(userIdInt);
        
            IEnumerable<FriendDto> friendDtos = _friendMapper.ToDto(friends);

            return Ok(friendDtos);
        }

        [Authorize]
        [HttpPost ("makerequest")]
        public async Task<ActionResult> requestFriendship (int friendId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out var userIdInt))
            {
                return Unauthorized("El usuario no está autenticado.");
            }

            Friendship friendship = await _friendshipService.requestFriendship(userIdInt, friendId);

            var friendshipSocketMessage = new FriendshipSocketMessage<FriendshipRequestModel>
            {
                Data = new FriendshipRequestModel
                {
                    State = friendship.State,
                    UserId = friendship.UserId,
                    FriendId = friendship.FriendId,
                }
            };

            string message = JsonSerializer.Serialize(friendshipSocketMessage);



            WebSocketHandler handler = _webSocketNetwork.GetSocketByUserId(friendship.UserId);

            await handler.SendAsync(message);

            return Ok("ok") ;
        }

        [Authorize]
        [HttpGet("getallrequests")]
        public async Task<ActionResult<List<Friendship>>> getAllRequests()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out var userIdInt))
            {
                return Unauthorized("El usuario no está autenticado.");
            }

            List<Friendship> requests = await _friendshipService.requestsByUserId(userIdInt);

            return Ok(requests);
        }

        [Authorize]
        [HttpPost ("acceptrequest")]
        public async Task<ActionResult<IEnumerable<FriendDto>>> acceptRequest (int friendId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out var userIdInt))
            {
                return Unauthorized("El usuario no está autenticado.");
            }
            List<User> friends = await _friendshipService.acceptFriendship(userIdInt, friendId);

            IEnumerable<FriendDto> friendDtos = _friendMapper.ToDto(friends);

            return Ok(friendDtos);

        }

    }

}
