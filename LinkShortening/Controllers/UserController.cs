using LinkShortening.Models;
using LinkShortening.Services.Contracts;
using LoggerService.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LinkShortening.Controllers
{
    /*[Authorize]*/
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserServices _userServices;
        private readonly ILoggerManager _logger;

        public UserController(ILoggerManager logger, IUserServices userServices)
        {
            _logger = logger;
            _userServices = userServices;
        }

        /*     /// <summary>
             /// 
             /// </summary>
             /// <param name="user"></param>
             /// <returns></returns>
             [AllowAnonymous]
             [HttpPost]
             [ProducesResponseType(StatusCodes.Status201Created)]
             public async Task<ActionResult> Registration([FromBody] User user)
             {
                 await _userServices.CreateAsync(user);
                 return Ok();
             }*/

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public ActionResult Login([FromBody] User user)
        {
            var token = _userServices.Authorization(user.Password, user.UserName);
            if (token == null)
                return Unauthorized();
            var userToJs = user.UserName;
            return Ok(new { token, userToJs });
            /*      var resultAuthorization =  _userServices.Authorization(user.Password, user.UserName);
                  if (resultAuthorization == null)
                      return this.RedirectPermanent("d");
                  return this.RedirectPermanent("b");*/

        }


    }
}
