using chess4connect.DTOs;
using chess4connect.Models.Database.Entities;
using chess4connect.Services;
using Microsoft.AspNetCore.Authorization;
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

        [HttpPut ("{userId}/editrole")]
        public async Task<ActionResult<UserDto>> EditRole(int userId)
        {
            
            UserDto user = await _adminService.ChangeRole(userId);

            return Ok(user);
        }
        
        [HttpPut("{userId}/editstatus")]
        public async Task<ActionResult<UserDto>> EditStatus(int userId)
        {
            UserDto user = await _adminService.ChangeStatus(userId);

            return Ok(user);
        }
    }
}
