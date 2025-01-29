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


    }
}
