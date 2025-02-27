using chess4connect.DTOs;
using chess4connect.Mappers;
using chess4connect.Models.Database.Entities;

namespace chess4connect.Services
{
    public class AdminService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly AdminMapper _adminMapper;

        public AdminService(UnitOfWork unitOfWork, AdminMapper adminMapper)
        {
            _unitOfWork = unitOfWork;
            _adminMapper = adminMapper;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsers()
        {
            IEnumerable<User> users = await _unitOfWork.UserRepository.GetAllAsync();
            return _adminMapper.ToDto(users);
        }

        public async Task ChangeRole(int userId)
        {
            User user = await _unitOfWork.UserRepository.GetByIdAsync(userId);

            user.Role = user.Role.Equals("user") ? "admin" : "user";
        }



    }
}
