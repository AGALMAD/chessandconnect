using chess4connect.DTOs;
using chess4connect.Models.Games.Base;
using chess4connect.Models.Games;

namespace chess4connect.Mappers;

public class GameMapper
{
    public static GameDto ToDto(Game game)
    {
        return new GameDto
        {
            GameType = game.GameType,
            Pieces = game.Board?.GetAllPieces(),
            StartDate = game.StartDate
        };
    }

    public static Game ToModel(GameDto gameDto)
    {
        Game newGame = new Game
        {
            GameType = gameDto.GameType,
            Board = new BaseBoard(), 
            StartDate = gameDto.StartDate
        };

        newGame.Board.SetPieces(gameDto.Pieces);

        return newGame;
    }
}
