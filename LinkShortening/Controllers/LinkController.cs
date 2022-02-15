using LinkShortening.Models;
using LinkShortening.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace LinkShortening.Controllers
{
    [Route("api/Link")]
    [ApiController]
    public class LinkController : ControllerBase
    {
        private readonly ILinksService _linkService;

        public LinkController(ILinksService linkService) =>
            _linkService = linkService;

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<List<Link>> Get() =>
            await _linkService.GetAsync();

        [HttpGet("{id:length(24)}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Link>> Get(string id)
        {
            var link = await _linkService.GetAsync(id);
            if (link is null)
            {
                return NotFound();
            }
            return link;
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Post(Link newLink)
        {
            await _linkService.CreateAsync(newLink);
            return CreatedAtAction(nameof(Get), new { id = newLink.Id }, newLink);
        }

        [HttpPut("{id:length(24)}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Update(string id, Link updatedLink)
        {
            var link = await _linkService.GetAsync(id);
            if (link is null)
            {
                return NotFound();
            }
            updatedLink.Id = link.Id;
            await _linkService.UpdateAsync(id, updatedLink);
            return NoContent();
        }
        [HttpDelete("{id:length(24)}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Delete(string id)
        {
            var link = await _linkService.GetAsync(id);
            if (link is null)
            {
                return NotFound();
            }
            await _linkService.RemoveAsync(link.Id);
            return NoContent();
        }


    }
}
