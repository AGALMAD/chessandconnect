using chess4connect.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace chess4connect.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GameController : ControllerBase
{
    private GameService _gameService;

    public GameController(GameService gameService)
    {
        _gameService = gameService;
    }

}
