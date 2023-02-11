using BusinessLogic.Models;
using BusinessLogic.Services.Interfaces.Services;
using DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NewsForum.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]

    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> Registration([FromBody] Login user)
        {
            await _userService.CreateAccount(user);

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] Login user)
        {
            var loginResult = await _userService.SingIn(user);
            return Ok(Results.Json(new
            { access_token = loginResult.Token, username = loginResult.Name, id = loginResult.Id }));
        }

        [Authorize(Roles = Roles.adminRole)]
        [HttpPost]
        public async Task<IActionResult> ChangeRole([FromBody] NameAndRole user)
        {
            await _userService.ChangeUserRole(user);
            return Ok();
        }

        [Authorize(Roles = Roles.adminRole)]
        [HttpPost]
        public async Task<IActionResult> BanUser([FromBody] NameAndRole user)
        {
            await _userService.BanAccount(user);
            return Ok();
        }
        
        [Authorize(Roles = Roles.adminRole)]
        [HttpPost]
        public async Task<IActionResult> UnBanUser([FromBody] NameAndRole user)
        {
            await _userService.UnBanAccount(user);
            return Ok();
        }
        
        [Authorize(Roles = Roles.adminRole)]
        [HttpPost]
        public async Task<IActionResult> DeleteUser([FromBody] string name)
        {
            var User = new NameAndRole { Name = name };
            await _userService.DeleteAccount(User);
            return Ok();
        }
        
        [Authorize(Roles = Roles.adminRole)]
        [HttpGet]
        public async Task<IActionResult> GetUsersList()
        {
            return Ok(await _userService.GetUsersList());
        }
    }
}
