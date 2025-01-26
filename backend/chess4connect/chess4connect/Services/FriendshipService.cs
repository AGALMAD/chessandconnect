using chess4connect.Models.Database.Entities;
using Microsoft.AspNetCore.Authorization;

namespace chess4connect.Services
{
    public class FriendshipService
    {
        private readonly UnitOfWork _unitOfWork;

        public FriendshipService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<User>> GetAllUsersByNickname(string nickName)
        {
            return await _unitOfWork.UserRepository.GetUsersByUserName(nickName);
        }

        public async Task<List<User>> GetAllUserFriends(int userId)
        {
            User user = await _unitOfWork.UserRepository.GetUserById(userId);

            return user.Friends.ToList();
            
        }

        public async Task<Friendship> requestFriendship(int userId, int friendId)
        {
            User user = await _unitOfWork.UserRepository.GetByIdAsync(userId);

            User friend = await _unitOfWork.UserRepository.GetByIdAsync(friendId);


            Friendship request = new Friendship
            {
                UserId = user.Id,
                FriendId = friend.Id,
                State = Enums.FriendshipState.Pending
            };

            await _unitOfWork.FriendshipRepository.InsertAsync(request);

            friend.Requests.Add(request);

            await _unitOfWork.SaveAsync();
            return request;
        }

        public async Task<List<User>> acceptFriendship(int userId, int friendId)
        {
            User user = await _unitOfWork.UserRepository.GetByIdAsync(userId);

            User friend = await _unitOfWork.UserRepository.GetByIdAsync(userId);

            Friendship pendingFriendship = await _unitOfWork.FriendshipRepository.GetFriendshipByUsers(user.Id, friend.Id);
            
            pendingFriendship.State = Enums.FriendshipState.Accepted;

            user.Friends.Add(friend);
            friend.Friends.Add(user);

            user.Requests.Remove(pendingFriendship);
            await _unitOfWork.SaveAsync();

            return user.Friends;
        }
    }


}
