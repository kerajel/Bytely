using MongoDB.Driver;

namespace Bytely.Interfaces
{
    public interface ISequenceProvider
    {
        Task<long> GetNextSequenceValue<T>(IMongoCollection<T> collection) where T : class;
    }
}