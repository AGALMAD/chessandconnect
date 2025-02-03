using chess4connect.Enums;
using chess4connect.Models.Database.Entities;
using chess4connect.Models.SocketComunication.Handlers;
using chess4connect.Models.SocketComunication.MessageTypes;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace chess4connect.Services
{
    public class MatchMakingService
    {
        private UnitOfWork _unitOfWork; 
        private WebSocketNetwork _webSocketNetwork;


        public MatchMakingService(UnitOfWork unitOfWork, WebSocketNetwork webSocketNetwork) 
        {
            _unitOfWork = unitOfWork;
            _webSocketNetwork = webSocketNetwork;
        }


        public async Task GameInvitation(int userId, int friendId, FriendshipState state)
        {
            WebSocketHandler friendHandler = _webSocketNetwork.GetSocketByUserId(friendId);

            if (friendHandler != null)
            {
                var gameInvitation = new SocketMessage<GameInvitationModel>
                {
                    Type = SocketCommunicationType.GAME_INVITATION,

                    Data = new GameInvitationModel
                    {
                        HostId = userId,
                        FriendId = friendId,
                        State = state,

                    }
                };

                string stringGamenInvitationMessage = JsonSerializer.Serialize(gameInvitation);

                await friendHandler.SendAsync(stringGamenInvitationMessage);


            }

        }




    }
}
