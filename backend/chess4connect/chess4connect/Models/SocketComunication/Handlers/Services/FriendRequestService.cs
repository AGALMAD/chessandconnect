using chess4connect.Models.Database.Entities;
using chess4connect.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace chess4connect.Models.SocketComunication.Handlers.Services
{
    public class FriendRequestService
    {
        private readonly FriendshipService _friendshipService;

        public FriendRequestService(FriendshipService friendshipService)
        {
            _friendshipService = friendshipService;
        }

        public async Task<ActionResult<Friendship>> requestFriendship(int userId, int requestedId)
        {
            Friendship friendship = await _friendshipService.requestFriendship(requestedId, userId);

            return friendship;
        }



    }
}
