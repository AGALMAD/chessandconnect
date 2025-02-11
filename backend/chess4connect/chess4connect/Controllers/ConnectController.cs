using chess4connect.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace chess4connect.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ConnectController : ControllerBase
{
    private ConnectService _connectService;

    public ConnectController(ConnectService connectService)
    {
        _connectService = connectService;
    }   


}
