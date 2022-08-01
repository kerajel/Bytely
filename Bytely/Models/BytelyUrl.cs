using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Bytely.Models
{
    public class BytelyUrl
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId _id { get; set; }

        public long UrlId { get; set; }

        public string? OriginUrl { get; set; }

        public string? ShortUrl { get; set; }

        public long RedirectCount { get; set; }

        public IList<string>? UserGuids { get; set; }
    }
}
