using LinkShortening.Models;

namespace LinkShortening.Services.Contracts
{
    public interface ILinksService
    {
        public Task<List<Link>> GetAsync();
        public Task<Link?> GetAsync(string id);
        public Task CreateAsync(Link newLink);
        public Task UpdateAsync(string id, Link updatedLink);
        public Task RemoveAsync(string id);
    }
}
