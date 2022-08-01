using Bytely.Models.Sequence;
using MongoDB.Driver;

namespace Bytely.Models.MongoContext
{
    public interface IMongoContext
    {
        IMongoClient? Client { get; set; }
        IMongoDatabase? Database { get; set; }
        IMongoCollection<SequenceEntry>? Sequence { get; set; }
        IMongoCollection<BytelyUrl>? BytelyUrl { get; set; }
    }
}