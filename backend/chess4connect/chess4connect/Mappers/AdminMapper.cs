using chess4connect.DTOs;
using chess4connect.Models.Database.Entities;

namespace chess4connect.Mappers
{
    public class AdminMapper
    {
        private readonly UnitOfWork _unitOfWork;

        public AdminMapper(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public UserDto ToDto(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Role = user.Role,
                AvatarImageUrl = user.AvatarImageUrl,
                Banned = user.Banned
            };
        }

        public IEnumerable<UserDto> ToDto(IEnumerable<User> users)
        {
            return users.Select(ToDto);
        }

    }
}
