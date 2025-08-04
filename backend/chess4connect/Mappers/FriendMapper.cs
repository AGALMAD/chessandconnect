using chess4connect.DTOs;
using chess4connect.Models.Database.Entities;
using chess4connect.Services;
using System.Numerics;

namespace chess4connect.Mappers;

public class FriendMapper
{
    private readonly UnitOfWork _unitOfWork;

    public FriendMapper(UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public FriendDto ToDto(User friend)
    {
        return new FriendDto
        {
            Id = friend.Id,
            UserName = friend.UserName,
            AvatarImageUrl = friend.AvatarImageUrl,
            Plays = friend.Plays,

        };
    }

    public IEnumerable<FriendDto> ToDto(IEnumerable<User> friends)
    {
        return friends.Select(ToDto);
    }


    public async Task<User> ToEntity(FriendDto friendDto)
    {
        User completeFriend = await _unitOfWork.UserRepository.GetByIdAsync(friendDto.Id);
        return completeFriend;
    }

    public async Task<IEnumerable<User>> ToEntity(IEnumerable<FriendDto> friendsDto)
    {
        List<User> friends = new List<User>();

        foreach (var friendDto in friendsDto)
        {
            friends.Add(await ToEntity(friendDto));
        }


        return friends;

    }
}
