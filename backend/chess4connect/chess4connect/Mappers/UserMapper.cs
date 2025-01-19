using chess4connect.Models.Database.DTOs;
using chess4connect.Models.Database.Entities;
using chess4connect.Services;

namespace chess4connect.Mappers;

public class UserMapper
{
    //Pasar de usuario a dto
    public UserAfterLoginDto ToDto(User user)
    {
        return new UserAfterLoginDto
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            Role = user.Role,
            AvatarImageUrl = user.AvatarImageUrl,
            Banned = user.Banned,
            Plays = user.Plays,
        };
    }

    //Pasar la lista de usuarios a dtos
    public IEnumerable<UserAfterLoginDto> ToDto(IEnumerable<User> users)
    {
        return users.Select(ToDto);
    }


    //Pasar de Dto a usuario
    public User ToEntity(UserAfterLoginDto userDto)
    {
        return new User
        {
            Id = userDto.Id,
            UserName = userDto.UserName,
            Email = userDto.Email,
            Role = userDto.Role,
            AvatarImageUrl = userDto.AvatarImageUrl,
            Banned = userDto.Banned,
            Plays = userDto.Plays,
        };
    }

    public User ToEntity(UserSignUpDto userDto)
    {
        PasswordService passwordService = new PasswordService();

        return new User
        {
            UserName = userDto.UserName,
            Email = userDto.Email,
            Password = passwordService.Hash(userDto.Password),
            Role = "user",
            AvatarImageUrl = "",
            Banned = false,
        };
    }


    //Pasar la lista de dtos a usuarios
    public IEnumerable<User> ToEntity(IEnumerable<UserAfterLoginDto> usersDto)
    {
        return usersDto.Select(ToEntity);
    }

    public IEnumerable<User> ToEntity(IEnumerable<UserSignUpDto> usersDto)
    {
        return usersDto.Select(ToEntity);
    }
}
