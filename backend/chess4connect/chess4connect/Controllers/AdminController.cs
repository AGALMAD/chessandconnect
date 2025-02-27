using chess4connect.DTOs;
using chess4connect.Models.Database.Entities;
using chess4connect.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace chess4connect.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private AdminService _adminService;

        public AdminController(AdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet ("allusers")]
        public async Task<IEnumerable<UserDto>> AllUsers()
        {
            return await _adminService.GetAllUsers();
        }
    }
}
