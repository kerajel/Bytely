using Bytely.Core.Interfaces;
using Bytely.Interfaces;
using Bytely.Models;
using Bytely.Models.MongoContext;
using MongoDB.Driver;
using System.Collections.Immutable;

namespace Bytely.Core.Services
{
    public class BytelytUrlService : IBytelyUrlService
    {
        private readonly IMongoContext _context;
        private readonly IShortUrlConvertor _urlConvertor;
        private readonly ISequenceProvider _sequenceProvider;

        public BytelytUrlService(IMongoContext context, IShortUrlConvertor urlConvertor, ISequenceProvider sequenceProvider)
        {
            _context = context;
            _urlConvertor = urlConvertor;
            _sequenceProvider = sequenceProvider;
        }

        /// <summary>
        /// Pretend we use real transactions on a replica-set
        /// </summary>
        private class FakeMongoTransaction : IDisposable
        {
            public void Dispose()
            {

            }
        }

        public async Task<string> GetBytelyUrl(Uri originUri, string? userGuid = null)
        {
            using var transaction = new FakeMongoTransaction();

            var bytelyUrl = await GetBytelyUrl(originUri.OriginalString, userGuid);

            if (bytelyUrl is null)
                bytelyUrl = await CreateBytelyUrl(originUri.OriginalString, userGuid);

            return bytelyUrl.ShortUrl!;
        }

        public async Task<IDictionary<string, long>> GetUserUrls(string? userGuid)
        {
            if (userGuid is null)
                return ImmutableDictionary<string, long>.Empty;

            var filter = Builders<BytelyUrl>.Filter.AnyIn(r => r.UserGuids, new[] { userGuid });
            var fields = Builders<BytelyUrl>.Projection
                .Include(r => r.ShortUrl)
                .Include(r => r.RedirectCount);
            var cursor = await _context.BytelyUrl.Find(filter)
                .Project<BytelyUrl>(fields)
                .ToListAsync();
            return cursor.ToDictionary(r => r.ShortUrl!, r => r.RedirectCount);
        }

        public async Task<string?> GetOriginUrl(long urlId)
        {
            var filter = Builders<BytelyUrl>.Filter.Eq(r => r.UrlId, urlId);
            var update = Builders<BytelyUrl>.Update.Inc(r => r.RedirectCount, 1);

            var bytelyUrl = await _context.BytelyUrl.FindOneAndUpdateAsync(filter, update);
            return bytelyUrl?.OriginUrl;
        }

        private async Task<BytelyUrl> CreateBytelyUrl(string originUrl, string? userGuid = null)
        {
            var shortUrlId = await _sequenceProvider.GetNextSequenceValue(_context.BytelyUrl!);
            var shortUrl = new BytelyUrl()
            {
                OriginUrl = originUrl,
                UrlId = shortUrlId,
                UserGuids = new List<string>(),
                ShortUrl = _urlConvertor.ConvertIdToShortUrl(shortUrlId)
            };

            if (userGuid != null)
                shortUrl.UserGuids.Add(userGuid);

            await _context.BytelyUrl!.InsertOneAsync(shortUrl);
            return shortUrl;
        }

        private async Task<BytelyUrl> GetBytelyUrl(string originUrl, string? userGuid = null)
        {
            var filter = Builders<BytelyUrl>.Filter.Eq(r => r.OriginUrl, originUrl);

            if (userGuid is null)
            {
                return await _context.BytelyUrl.Find(filter).FirstOrDefaultAsync();
            }
            else
            {
                var update = Builders<BytelyUrl>.Update.AddToSet(r => r.UserGuids, userGuid);
                return await _context.BytelyUrl.FindOneAndUpdateAsync(filter, update);
            }
        }
    }
}