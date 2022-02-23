using LinkShortening.Models;
using LinkShortening.Services.Contracts;
using LinkShortening.Settings.Contracts;
using MongoDB.Driver;

namespace LinkShortening.Services
{

    public class UrlServices : IUrlServices
    {
        private readonly IMongoCollection<Url> _linksCollection;
        public UrlServices(
            ILinkShorteningDatabaseSettings linkShorteningDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                linkShorteningDatabaseSettings.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                linkShorteningDatabaseSettings.DatabaseName);

            _linksCollection = mongoDatabase.GetCollection<Url>(
                linkShorteningDatabaseSettings.LinksCollectionNameUrl);
        }
        public async Task<Url> Click(string segment)
        {
            var url = await _linksCollection.Find(x => x.Segment == segment).FirstOrDefaultAsync();
            if (url == null) return null;
            url.NumOfClicks++;
            await _linksCollection.ReplaceOneAsync(x => x.Id == url.Id, url);
            return url;
        }
        public async Task<Url> GetOnLongUrlAsync(string longUrl, string userName)
        {
            var url = await _linksCollection.Find(x => x.ShortURL == longUrl && x.UserId == userName).FirstOrDefaultAsync();
            if (url == null) return null;
            url.NumOfClicks += 1;
            await _linksCollection.ReplaceOneAsync(x => x.Id == url.Id, url);
            return url;
        }
        public async Task CreateAsync(Url newLink) =>
            await _linksCollection.InsertOneAsync(newLink);
        public async Task<Url> ShortenUrl(string longUrl, string ip, string userName, string segment = "")
        {
            var shortLink = await _linksCollection.Find(x => x.LongURL == longUrl).FirstOrDefaultAsync();
            if (shortLink != null) return shortLink;
            string newSegment = await this.NewSegment();
            var url = new Url
            {
                Ip = ip,
                LongURL = longUrl,
                NumOfClicks = 0,
                Segment = newSegment,
                UserId = userName

            };
            return url;
        }

        private async Task<string> NewSegment()
        {
            while (true)
            {
                string? segment = Guid.NewGuid().ToString().Substring(0, 6);
                var url = await _linksCollection.Find(x => x.Segment == segment).FirstOrDefaultAsync();

                if (url == null)
                {
                    return segment;
                }
            }
        }
        public async Task<List<Url>> GetAsync(string id) =>
            await _linksCollection.Find(x => x.UserId == id && x.Incognito == false).ToListAsync();
        public async Task UpdateAsync(Url updatedUrl) =>
            await _linksCollection.ReplaceOneAsync(x => x.Id == updatedUrl.Id, updatedUrl);

    }
}
