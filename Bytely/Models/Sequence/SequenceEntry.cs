using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Bytely.Models.Sequence
{
    public class SequenceEntry
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public string? Name { get; set; }

        public long Value { get; set; }
    }
}
