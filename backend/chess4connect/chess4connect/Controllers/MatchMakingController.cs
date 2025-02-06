using chess4connect.Enums;
using chess4connect.Models.Database.DTOs;
using chess4connect.Models.Database.Entities;
using chess4connect.Models.SocketComunication.Handlers;
using chess4connect.Models.SocketComunication.MessageTypes;
using chess4connect.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;
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

        [HttpPost("queueGame")]
        public async Task<ActionResult> QueueGame(Game gamemode)
        {

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            await _queueService.AddToQueueAsync(userId, gamemode);

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



        [Authorize]
        [HttpPost("start")]
        public async Task<ActionResult> StartPlay([FromQuery] int opponentId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out var userIdInt))
            {
                return Unauthorized("El usuario no está autenticado.");
            }

            //Notifica al oponente del inicio de partida


            return Ok("Partida creada");

        }


        [Authorize]
        [HttpPost("end")]
        public async Task<ActionResult> EndPlay([FromBody] GameRequest request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out var userIdInt))
            {
                return Unauthorized("El usuario no está autenticado.");
            }

            //Guarda la partida en la base de datos y notifica al oponente




            return Ok("Partida creada");

        }

        [Authorize]
        [HttpPost("cancelQueue")]
        public async Task<ActionResult> CancelQueue([FromBody] Game game)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out var userIdInt))
            {
                return Unauthorized("El usuario no está autenticado.");
            }

            await _queueService.cancelGame(userIdInt, game);

            return Ok("Eliminado de la cola");
        }

 
    }
}
