using Bytely.Interfaces;
using Bytely.Models.MongoContext;
using Bytely.Models.Sequence;
using MongoDB.Driver;

namespace Bytely.Services
{
    public class SequenceProvider : ISequenceProvider
    {
        private readonly IMongoContext _context;

        public SequenceProvider(IMongoContext context)
        {
            _context = context;
        }

        public async Task<long> GetNextSequenceValue<T>(IMongoCollection<T> collection) where T : class
        {
            var filter = Builders<SequenceEntry>.Filter.Eq(r => r.Name, collection.CollectionNamespace.CollectionName);
            var update = Builders<SequenceEntry>.Update.Inc(r => r.Value, 1);
            var entry = await _context.Sequence.FindOneAndUpdateAsync(filter, update);
            return entry.Value;
        }
    }
}
