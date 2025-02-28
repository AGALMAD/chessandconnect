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

        public async Task<UserDto> ChangeRole(int userId)
        {
            User user = await _unitOfWork.UserRepository.GetByIdAsync(userId);

            user.Role = user.Role.Equals("user") ? "admin" : "user";

            _unitOfWork.Context.Users.Update(user);
            await _unitOfWork.SaveAsync();

            return _adminMapper.ToDto(user);
        }

        public async Task<UserDto> ChangeStatus (int userId)
        {
            User user = await _unitOfWork.UserRepository.GetByIdAsync(userId);

            if (!user.Banned)
            {
                user.Banned = true;
            } else
            {
                user.Banned = false;
            }

            _unitOfWork.Context.Users.Update(user);
            await _unitOfWork.SaveAsync();

            return _adminMapper.ToDto(user);
        }


    }
}
