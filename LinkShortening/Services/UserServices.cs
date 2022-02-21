using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LinkShortening.Models;
using LinkShortening.Services.Contracts;
using LinkShortening.Settings.Contracts;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;

namespace LinkShortening.Services
{
    public class UserServices : IUserServices
    {

        private readonly IMongoCollection<User> _linksCollection;
        public readonly string key;

        public UserServices(IConfiguration configuration, ILinkShorteningDatabaseSettings linkShorteningDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                linkShorteningDatabaseSettings.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                linkShorteningDatabaseSettings.DatabaseName);

            _linksCollection = mongoDatabase.GetCollection<User>(
                linkShorteningDatabaseSettings.LinksCollectionNameUsers);
            this.key = configuration.GetSection("JwtKey").ToString();

        }
        // configuration.GetSection("LinkShortening").GetConnectionString("ConnectionString"));

        public async Task<User> GerUserAsync(string id) =>
            await _linksCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(User newUser) =>
            await _linksCollection.InsertOneAsync(newUser);


        public string Authorization(string password, string userName)
        {

            var user = _linksCollection.Find(e => e.UserName == userName && e.Password == password).FirstOrDefault();

            if (user == null) return null;
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(key);
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email, userName), //rename username
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey),
                    SecurityAlgorithms.HmacSha256Signature)

            };
            var token = tokenHandler.CreateToken(tokenDescriptor);



            return tokenHandler.WriteToken(token);
        }


    }
}
