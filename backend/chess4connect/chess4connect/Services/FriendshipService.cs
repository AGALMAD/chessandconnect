using chess4connect.Models.Database.Entities;

namespace chess4connect.Services
{
    public class FriendshipService
    {
        private readonly UnitOfWork _unitOfWork;

        public FriendshipService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<User> GetUserByNickName(string nickName)
        {
            return await _unitOfWork.UserRepository.GetUserByUserName(nickName);
        }
    }

    
}
