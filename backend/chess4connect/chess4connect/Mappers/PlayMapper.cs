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
            
            EndDate = play.EndDate,
          
           
            

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
           
            StartDate = playDto.StartDate,
            EndDate = playDto.EndDate,
            

        };
    }

    public IEnumerable<Play> ToEntity(IEnumerable<PlayDto> playsDto)
    {
        return playsDto.Select(ToEntity);

    }
}
