using chess4connect.DTOs;
using chess4connect.Models.Database.Entities;
using chess4connect.Services;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<ActionResult> sendChatMessage([FromBody] Chat chat)
        {
            await _chatService.sendMessage(chat);

            return Ok("Mensage enviado");
        }
    }
}
