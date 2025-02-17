using chess4connect.DTOs;
using chess4connect.Models.Database.Entities;
using chess4connect.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace chess4connect.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : Controller
    {
        private ChatService _chatService;

        public ChatController(ChatService chatService) {
            _chatService = chatService;
        }

        [HttpPost("Chat")]
        public async Task<ActionResult> SendChatMessage([FromBody] string message)
        {

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            await _chatService.sendMessage(message, userId);

            return Ok("Mensage enviado");
        }
    }
}
