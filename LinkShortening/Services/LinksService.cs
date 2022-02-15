using LinkShortening.Models;
using LinkShortening.Services.Contracts;
using LinkShortening.Settings.Contracts;
using MongoDB.Driver;

namespace LinkShortening.Services
{
    public class LinksService : ILinksService
    {
        private readonly IMongoCollection<Link> _linksCollection;

        public LinksService(
            ILinkShorteningDatabaseSettings linkShorteningDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                linkShorteningDatabaseSettings.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                linkShorteningDatabaseSettings.DatabaseName);

            _linksCollection = mongoDatabase.GetCollection<Link>(
                linkShorteningDatabaseSettings.LinksCollectionName);
        }

        public async Task<List<Link>> GetAsync() =>
            await _linksCollection.Find(_ => true).ToListAsync();

        public async Task<Link?> GetAsync(string id) =>
            await _linksCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Link newLink) =>
            await _linksCollection.InsertOneAsync(newLink);

        public async Task UpdateAsync(string id, Link updatedLink) =>
            await _linksCollection.ReplaceOneAsync(x => x.Id == id, updatedLink);

        public async Task RemoveAsync(string id) =>
            await _linksCollection.DeleteOneAsync(x => x.Id == id);
    }
}
