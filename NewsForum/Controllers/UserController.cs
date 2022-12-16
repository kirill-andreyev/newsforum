using BusinessLogic.Models;
using BusinessLogic.Services.Interfaces.Services;
using DataAccess.Models;
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
        public async Task<IActionResult> Registration([FromBody] string name, string password)
        {
            var User = new Login { Name = name, Password = password };

            await _userService.CreateAccount(User);

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] Login user)
        {
            var loginResult = await _userService.SingIn(user);
            return Ok(Results.Json(new
            { access_token = loginResult.Token, username = loginResult.Name, id = loginResult.Id }));
        }
    }
}
