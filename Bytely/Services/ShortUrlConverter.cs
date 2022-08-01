using Bytely.Core.Interfaces;
using Bytely.Models.Settings;
using Microsoft.Extensions.Options;

namespace Bytely.Core.Services
{
    public class ShortUrlConverter : IShortUrlConvertor
    {
        private readonly UrlProviderSettings _settings;

        public ShortUrlConverter(IOptions<UrlProviderSettings> settingsOptions)
        {
            _settings = settingsOptions.Value;
        }

        public string ConvertIdToShortUrl(long urlId)
        {
            return string.Format(_settings.UrlTemplate!, _settings.UrlPort, urlId.ToString("X"));
        }

        public bool TryConvertUriLocalPathToId(string url, out long urlId)
        {
            try
            {
                urlId = Convert.ToInt64(url, 16);
                return true;
            }
            catch
            {
                urlId = -1;
                return false;
            }
        }
    }
}
