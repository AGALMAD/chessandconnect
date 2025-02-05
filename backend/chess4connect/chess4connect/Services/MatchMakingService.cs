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


        public async Task GameInvitation(GameInvitationModel gameInvitation)
        {
            
            var gameInvitationMessage = new SocketMessage<GameInvitationModel>
            {
                Type = SocketCommunicationType.GAME_INVITATION,

                Data = new GameInvitationModel
                {
                    HostId = gameInvitation.HostId,
                    FriendId = gameInvitation.FriendId,
                    State = gameInvitation.State,
                    Game = gameInvitation.Game,

                }
            };

            string stringGamenInvitationMessage = JsonSerializer.Serialize(gameInvitationMessage);

            //Si se envia la invitación, el socket a enviar es el del amigo
            if (gameInvitation.State == FriendshipState.Pending) {
                WebSocketHandler friendHandler = _webSocketNetwork.GetSocketByUserId(gameInvitation.FriendId);
                await friendHandler.SendAsync(stringGamenInvitationMessage);
            }
            //Si se acepta, el socket a enviar es el del host
            else
            {
                WebSocketHandler friendHandler = _webSocketNetwork.GetSocketByUserId(gameInvitation.HostId);
                await friendHandler.SendAsync(stringGamenInvitationMessage);

            }

        }




    }
}
