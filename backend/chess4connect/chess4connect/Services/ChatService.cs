using chess4connect.DTOs;
using chess4connect.DTOs.Games;
using chess4connect.Enums;
using chess4connect.Models.Database.Entities;
using chess4connect.Models.Games.Chess.Chess;
using chess4connect.Models.Games.Chess.Chess.Pieces;
using chess4connect.Models.Games.Connect;
using chess4connect.Models.SocketComunication.Handlers;
using chess4connect.Models.SocketComunication.MessageTypes;
using System.Security.Claims;
using System.Text.Json;

namespace chess4connect.Services
{
    public class ChatService
    {
        private WebSocketNetwork _network;
        private RoomService _room;
        public ChatService(WebSocketNetwork network) {
            _network = network;
        }

        public async Task sendMessage(string message, int userId) {

            int player2Id;

            if (_room.GetChessRoomByUserId(userId) == null)
            {
                ConnectRoom room = _room.GetConnectRoomByUserId(userId);

                player2Id = getPlayer2Id(userId, room.Player1Id, room.Player2Id);
            }
            else
            {
                ChessRoom room = _room.GetChessRoomByUserId(userId);

                player2Id = getPlayer2Id(userId, room.Player1Id, room.Player2Id);

            }

            WebSocketHandler socketPlayer2 = _network.GetSocketByUserId(player2Id);

            var chatMessage = new SocketMessage<Chat>
            {
                Type = SocketCommunicationType.CHAT,

                Data = new Chat
                {
                    Message = message,
                }
            };

            string chat = JsonSerializer.Serialize(chatMessage);

            //Envia los mensajes a los jugadores

            if (socketPlayer2 != null)
            {
                await socketPlayer2.SendAsync(chat);
            }
        }

        private int getPlayer2Id(int userId, int player1Id, int player2Id)
        {

            int id;

            if (player1Id != userId)
            {
                id = player1Id;
            }
            else
            {
                id = player2Id;
            }

            return id;
        }
    }


}
