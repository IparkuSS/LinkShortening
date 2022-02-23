using LinkShortening.Models;

namespace LinkShortening.Services.Contracts
{
    public interface IUrlServices
    {
        public Task CreateAsync(Url newLink);
        public Task<List<Url>> GetAsync(string id);
        public Task<Url> Click(string shortUrl);
        public Task<Url> ShortenUrl(string longUrl, string ip, string userName, string segment = "");
        public Task<Url> GetOnLongUrlAsync(string longUrl, string userName);
        public Task UpdateAsync(Url updatedUrl);
    }
}
