using LinkShortening.Models;

namespace LinkShortening.Services.Contracts
{
    public interface IUserServices
    {
        public Task<User> GerUserAsync(string id);

        public Task CreateAsync(User newUser);

        public string Authorization(string password, string userName);
    }
}
