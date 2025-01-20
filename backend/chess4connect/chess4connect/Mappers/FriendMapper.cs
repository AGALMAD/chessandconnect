using chess4connect.Models.Database.DTOs;
using chess4connect.Models.Database.Entities;
using chess4connect.Services;
using System.Numerics;

namespace chess4connect.Mappers;

public class FriendMapper
{
    public PlayDto ToDto(Play play)
    {
        return new PlayDto
        {
            Id = play.Id,
            User = play.User,
            Opponent = play.Opponent,
            StartDate = play.StartDate,
            EndDate = play.EndDate,
            PlayState = play.PlayState,
            Game = play.Game,

        };
    }

    public IEnumerable<PlayDto> ToDto(IEnumerable<Play> plays)
    {
        return plays.Select(ToDto);
    }


    public Play ToEntity(PlayDto playDto)
    {
        return new Play
        {
            Id = playDto.Id,
            UserId = playDto.User.Id,
            User = playDto.User,
            OpponentId = playDto.Opponent.Id,
            Opponent = playDto.Opponent,
            StartDate = playDto.StartDate,
            EndDate = playDto.EndDate,
            PlayState = playDto.PlayState,
            GameId = playDto.Game.Id,
            Game = playDto.Game,

        };
    }

    public IEnumerable<Play> ToEntity(IEnumerable<PlayDto> playsDto)
    {
        return playsDto.Select(ToEntity);

    }
}
