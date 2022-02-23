using LinkShortening.Models;
using LinkShortening.Services.Contracts;
using LoggerService.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LinkShortening.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserServices _userServices;
        private readonly ILoggerManager _logger;

        public UserController(ILoggerManager logger, IUserServices userServices)
        {
            _logger = logger;
            _userServices = userServices;
        }

        /// <summary>
        /// method registers the user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("registration")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Registration()
        {

            var user = await _userServices.CreateAsync();
            var userName = user.UserName;
            var password = user.Password;
            return Ok(new { userName, password });
        }

        /// <summary>
        /// the method authorizes and issues a token
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult Login([FromBody] User userAuteres)
        {
            var token = _userServices.Authorization(userAuteres.Password, userAuteres.UserName);
            if (token == null)
                return Unauthorized();
            var user = userAuteres.UserName;
            var incog = userAuteres.Incognito;
            return Ok(new { token, user, incog });
        }
    }
}
