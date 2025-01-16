using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace chess4connect.Controllers;

[Route("socket")]
[ApiController]
public class WebSocketController : ControllerBase
{

    [HttpGet]

    public async Task ConnectAsync()
    {

    }

}
