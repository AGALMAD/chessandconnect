using chess4connect.Models.Database.DTOs;
using chess4connect.Models.Database.Entities;
using chess4connect.Models.SocketComunication.Handlers;
using chess4connect.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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

        public MatchMakingController(WebSocketNetwork webSocketNetwork,UserService userService, 
            MatchMakingService matchMakingService)
        {
            _webSocketNetwork = webSocketNetwork;
            _userService = userService;
            _matchMakingService = matchMakingService;
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



    }
}
