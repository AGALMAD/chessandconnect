using chess4connect.Models.Database.DTOs;
using chess4connect.Models.Database.Entities;

namespace chess4connect.Mappers;

public class PlayMapper
{
    public PlayDto ToDto(Play play)
    {
        return new PlayDto
        {
            Id = play.Id,
            GameId = play.GameId,
            StartDate = play.StartDate,
            EndDate = play.EndDate,
            PlayState = play.PlayState,
            Game = play.Game,
            Players = play.Players,
            

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
            UserId = playDto.Id,
            OpponentId = playDto.Id,
            StartDate = playDto.StartDate,
            EndDate = playDto.EndDate,
            PlayState = playDto.PlayState,
            GameId = playDto.Game.Id,
            Game = playDto.Game,
            Players = playDto.Players,

        };
    }

    public IEnumerable<Play> ToEntity(IEnumerable<PlayDto> playsDto)
    {
        return playsDto.Select(ToEntity);

    }
}
