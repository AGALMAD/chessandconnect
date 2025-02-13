using chess4connect.Models.Games;
using chess4connect.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

    [HttpPost("moveChessPiece")]
    public async Task<ActionResult> MoveChessPiece(ChessMoveRequest chessMoveRequest)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

        if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out var userIdInt))
        {
            return Unauthorized("El usuario no está autenticado.");
        }

        await _gameService.MoveChessPiece(chessMoveRequest, Int32.Parse(userId));

        return Ok(chessMoveRequest);
    }


    [HttpPost("moveConnectPiece")]
    public async Task<ActionResult> moveConnectPiece(ChessMoveRequest chessMoveRequest)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

        if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out var userIdInt))
        {
            return Unauthorized("El usuario no está autenticado.");
        }



        return Ok(chessMoveRequest);

    }





}
