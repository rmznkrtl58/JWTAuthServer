using AuthServer.Core.DTOs;
using AuthServer.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AuthServer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : CustomBaseController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserDto userDto)
        {
            var result = await _userService.CreateUserAsync(userDto);
            return ActionResultInstance(result);
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            var findUser = await _userService.GetUserByUsernameAsync(User.Identity.Name);
            return ActionResultInstance(findUser);
        }
        [HttpPost("CreateUserRoles/{userName}")]
        public async Task<IActionResult> CreateUserRoles(string userName)
        {
            var result = await _userService.CreateUserRole(userName);
            return ActionResultInstance(result);
        }
    }
}
