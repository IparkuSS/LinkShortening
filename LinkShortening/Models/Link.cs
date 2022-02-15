using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace LinkShortening.Models
{
    public class Link
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        [BsonElement("FullLink")]
        [JsonPropertyName("FullLink")]
        public string? FullLink { get; set; }
        [BsonElement("ShortLink")]
        [JsonPropertyName("ShortLink")]
        public string? ShortLink { get; set; }
    }
}
