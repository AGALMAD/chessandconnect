using chess4connect.DTOs;
using chess4connect.DTOs.Games;
using chess4connect.Enums;
using chess4connect.Models.Database.Entities;
using chess4connect.Models.SocketComunication.Handlers;
using chess4connect.Models.SocketComunication.MessageTypes;
using System.Text.Json;

namespace chess4connect.Services
{
    public class ChatService
    {
        private WebSocketNetwork _network;
        public ChatService(WebSocketNetwork network) {
            network = _network;
        }

        public async Task sendMessage(Chat chat) {

            WebSocketHandler socketPlayer = _network.GetSocketByUserId(chat.PlayerId);

            var chatMessage = new SocketMessage<Chat>
            {
                Type = SocketCommunicationType.CHAT,

                Data = new Chat
                {
                    PlayerId = chat.PlayerId,
                    Message = chat.Message,
                }
            };

            string message = JsonSerializer.Serialize(chatMessage);

            //Envia los mensajes a los jugadores

            if (socketPlayer != null)
            {
                await socketPlayer.SendAsync(message);
            }
        }
    }


}
