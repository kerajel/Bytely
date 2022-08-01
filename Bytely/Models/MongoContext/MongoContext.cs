using Bytely.Models;
using Bytely.Models.Sequence;
using MongoDB.Driver;

namespace Bytely.Models.MongoContext
{
    public class MongoContext : IMongoContext
    {
        public IMongoClient? Client { get; set; }

        public IMongoDatabase? Database { get; set; }

        public IMongoCollection<BytelyUrl>? BytelyUrl { get; set; }

        public IMongoCollection<SequenceEntry>? Sequence { get; set; }

    }
}
