using chess4connect.Mappers;
using chess4connect.Models.Database.Entities;
using chess4connect.Services;
using Microsoft.AspNetCore.Mvc;

namespace chess4connect.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private UnitOfWork _unitOfWork;
        private UserService _service;
        private SmartSearch _smartSearch;
        public UserController(UserService userService, UnitOfWork unitOfWork, SmartSearch smartSearch)
        {
            _service = userService;
            _unitOfWork = unitOfWork;
            _smartSearch = smartSearch;

        }

        [HttpGet("searchUser")]
        public List<User> getAllUsers([FromQuery] string query)
        {
            return (List<User>)_smartSearch.Search(query);
        }
    }
}
