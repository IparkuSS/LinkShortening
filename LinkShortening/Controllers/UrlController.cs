using LinkShortening.Models;
using LinkShortening.Services.Contracts;
using LoggerService.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LinkShortening.Controllers
{

    [Route("api/url")]
    [ApiController]
    public class UrlController : ControllerBase
    {
        private readonly IUrlServices _urlServices;
        private readonly ILoggerManager _logger;
        public UrlController(ILoggerManager logger, IUrlServices urlServices)
        {
            _urlServices = urlServices;
            _logger = logger;
        }
        /// <summary>
        /// method returns all links
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>

        [Route("urls/{user}")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<List<Url>> Get(string user) =>
           await _urlServices.GetAsync(user);

        /// <summary>
        /// method transition the url
        /// </summary>
        /// <param name="segment"></param>
        /// <returns></returns>
        [HttpGet("{segment}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Transition(string segment)
        {
            var url = await _urlServices.Click(segment);
            if (url == null)
            {
                _logger.LogError("url object is null");
                return BadRequest("url object is null");
            }
            return this.RedirectPermanent(url.LongURL);
        }

        /// <summary>
        /// method returns long urls
        /// </summary>
        /// <param name="shortURL"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        [Route("long/{userName}")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetOnLongUrl([FromBody] string shortURL, string userName)
        {
            var urlTemp = await _urlServices.GetOnLongUrlAsync(shortURL, userName);
            if (urlTemp == null)
            {
                _logger.LogError("url object is null");
                return BadRequest("url object is null");
            }
            var longUr = urlTemp.LongURL;
            return Ok(new { longUr });
        }

        /// <summary>
        /// method shortens and returns a link
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Post([FromBody] Url url)
        {
            if (url == null)
            {
                _logger.LogError("url object is null");
                return BadRequest("url object is null");
            }
            var shortUrl = await this._urlServices.ShortenUrl(url.LongURL, HttpContext.Connection.RemoteIpAddress.ToString(), url.UserId);

            if (shortUrl == null)
            {
                _logger.LogError("shortUrl object is null");
                return BadRequest("shortUrl object is null");
            }

            shortUrl.ShortURL = string.Format("{0}://{1}/api/url/{2}{3}", HttpContext.Request.Scheme, HttpContext.Request.Host, Url.Content("~"), shortUrl.Segment);
            shortUrl.Incognito = url.Incognito;
            await _urlServices.CreateAsync(shortUrl);
            var urlShortUrl = shortUrl.ShortURL;
            return Ok(new { urlShortUrl });
        }
    }
}