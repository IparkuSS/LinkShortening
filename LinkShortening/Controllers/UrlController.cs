using LinkShortening.Models;
using LinkShortening.Services.Contracts;
using LoggerService.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LinkShortening.Controllers
{
    /*[Authorize]*/
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
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<List<Url>> Get() =>
           await _urlServices.GetAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="segment"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("{segment}")]
        [ProducesResponseType(StatusCodes.Status200OK)]//to do 
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Get(string segment)
        {
            var shortUrl = await _urlServices.Click(segment);
            if (shortUrl == null)
            {
                _logger.LogError("shortUrl object is null");
                return BadRequest("shortUrl object is null");
            }
            return this.RedirectPermanent(shortUrl.LongURL);
        }

        /* /// <summary>
         /// 
         /// </summary>
         /// <param name="LongURL"></param>
         /// <returns></returns>
         [HttpGet("{LongURL}")]
         [ProducesResponseType(StatusCodes.Status200OK)]
         [ProducesResponseType(StatusCodes.Status404NotFound)]
         public async Task<Url> GetOnLongUrl(Url url) =>
             await _urlServices.GetOnLongUrlAsync(url.LongURL);*/

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult> Post([FromBody] Url url)
        {
            if (url == null)
            {
                _logger.LogError("url object is null");
                return BadRequest("url object is null");
            }
            var shortUrl = await this._urlServices.ShortenUrl(url.LongURL, HttpContext.Connection.RemoteIpAddress.ToString());

            if (shortUrl == null)
            {
                _logger.LogError("shortUrl object is null");
                return BadRequest("shortUrl object is null");
            }

            shortUrl.ShortURL = string.Format("{0}://{1}/{2}{3}", HttpContext.Request.Scheme, HttpContext.Request.Host, Url.Content("~"), shortUrl.Segment);
            await _urlServices.CreateAsync(shortUrl);
            return Ok();
        }
    }
}