namespace Bytely.Core.Interfaces
{
    public interface IShortUrlConvertor
    {
        string ConvertIdToShortUrl(long urlId);

        bool TryConvertUriLocalPathToId(string url, out long urlId);
    }
}
