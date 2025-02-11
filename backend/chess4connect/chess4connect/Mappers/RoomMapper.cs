using chess4connect.DTOs;
using chess4connect.Models.Games.Base;
using chess4connect.Models.Games;

namespace chess4connect.Mappers;

public class RoomMapper
{

    public static RoomDto ToDto(Room room)
    {
        return new RoomDto
        {
            Player1Id = room.Player1Id,
            Player2Id = room.Player2Id,
            Game = GameMapper.ToDto(room.Game)
        };
    }

    public static Room ToModel(RoomDto roomDto)
    {
        return new Room
        {
            Player1Id = roomDto.Player1Id,
            Player2Id = roomDto.Player2Id,
            Game = GameMapper.ToModel(roomDto.Game)
        };
    }
}
