using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace LinkShortening.Models
{
    public class Url
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        [BsonElement("LongURL")]
        [JsonPropertyName("LongURL")]
        public string LongURL { get; set; }
        [BsonElement("ShortURL")]
        [JsonPropertyName("ShortURL")]
        public string? ShortURL { get; set; }
        [BsonElement("Ip")]
        [JsonPropertyName("Ip")]
        public string? Ip { get; set; }
        [BsonElement("NumOfClicks")]
        [JsonPropertyName("NumOfClicks")]
        public int NumOfClicks { get; set; }
        [BsonElement("Segment")]
        [JsonPropertyName("Segment")]
        public string? Segment { get; set; }

        [BsonElement("UserId")]
        [JsonPropertyName("UserId ")]//recheck
        public string? UserId { get; set; }
    }
}
