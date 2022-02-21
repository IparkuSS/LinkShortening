using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace LinkShortening.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        [BsonElement("UserName")]
        public string? UserName { get; set; }
        [BsonElement("Password")]
        public string? Password { get; set; }

        [BsonElement("Incognito")]
        [JsonPropertyName("Incognito")]
        public bool? Incognito { get; set; }

    }
}
