using chess4connect.Enums;
using chess4connect.Models.SocketComunication.Handlers;
using chess4connect.Models.SocketComunication.MessageTypes;
using chess4connect.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace chess4connect.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchMakingController : ControllerBase
    {
        private WebSocketNetwork _webSocketNetwork;
        private UserService _userService;
        private MatchMakingService _matchMakingService;
        private QueueService _queueService;

        public MatchMakingController(WebSocketNetwork webSocketNetwork,UserService userService, 
            MatchMakingService matchMakingService, QueueService queueService)
        {
            _webSocketNetwork = webSocketNetwork;
            _userService = userService;
            _matchMakingService = matchMakingService;
            _queueService = queueService;
        }


        [Authorize]
        [HttpPost("queueGame")]
        public async Task<ActionResult> QueueGame([FromBody] GameService gamemode)
        {

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            await _queueService.AddToQueueAsync(userId, gamemode);

            return Ok("SI");
        }




        [Authorize]
        [HttpPost("cancelQueue")]
        public async Task<ActionResult> CancelQueue([FromBody] GameType game)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out var userIdInt))
            {
                return Unauthorized("El usuario no está autenticado.");
            }

            await _queueService.cancelGame(userIdInt, game);

            return Ok("Eliminado de la cola");
        }


        [Authorize]
        [HttpPost("IAGame")]
        public async Task<ActionResult> IAGame([FromBody] GameService gamemode)
        {

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            await _queueService.goIntoIAGame(userId, gamemode);

            return Ok("SI");
        }

        [Authorize]
        [HttpPost("FriendGame")]
        public async Task<ActionResult> FriendGame([FromBody] Room room)
        {

            var gamemode = room.Game;
            var friendId = room.Player2Id;

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            await _queueService.goIntoFriendGame(userId, (int)friendId, gamemode);

            return Ok("SI");
        }

        [HttpPost("newGameInvitation")]
        public async Task<ActionResult> GameInvitation([FromBody] GameInvitationModel gameInvitation)
        {
            //Si no es una usuario autenticado termina la ejecución
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId) || !long.TryParse(userId, out var userIdLong))
            {
                HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return Unauthorized("El usuario no está autenticado.");
            }

            await _matchMakingService.GameInvitation(gameInvitation);

            return Ok("Invitación Enviada");


        }


        [Authorize]
        [HttpPost("acceptInvitation")]
        public async Task<ActionResult> AcceptInvitation([FromBody] GameInvitationModel gameInvitation)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out var userIdInt))
            {
                return Unauthorized("El usuario no está autenticado.");
            }

            WebSocketHandler friendSocketHandler = _webSocketNetwork.GetSocketByUserId(userIdInt);

            //Envia el mensaje de aceptación al oponente
            await _matchMakingService.GameInvitation(gameInvitation);


            return Ok("Invitación aceptada");

        }

 
    }
}
