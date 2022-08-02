namespace Bytely.Core.Interfaces
{
    public interface IBytelyUrlService
    {
        Task<string> GetBytelyUrl(Uri originUri, string? userGuid = null);

        Task<IDictionary<string, long>> GetUserUrls(string? userGuid);

        Task<string?> GetOriginUrl(long urlId);
    }
}
