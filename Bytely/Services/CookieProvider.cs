using Bytely.Core.Interfaces;
using Bytely.Models.Settings;
using Microsoft.Extensions.Options;
using System.Net;

namespace Bytely.Core.Services
{
    public class CookieProvider : ICookieProvider
    {
        private readonly UserCookieSettings _settings;
        private readonly IDateTimeProvider _dateTimeProvider;

        public CookieProvider(IOptions<UserCookieSettings> settingsOptions, IDateTimeProvider dateTimeProvider)
        {
            _settings = settingsOptions.Value;
            _dateTimeProvider = dateTimeProvider;
        }

        public void AppendUserCookie(IResponseCookies responseCookies, out string? cookieValue)
        {
            cookieValue = GetNewCookieValue();
            var expires = _dateTimeProvider.UtcNow.Add(_settings.ExpiresIn);
            var cookieOptions = new CookieOptions()
            {
                Expires = expires
            };
            responseCookies.Append(_settings.CookieName!, cookieValue, cookieOptions);
        }

        public bool UserCookieExists(IRequestCookieCollection requestCookies, out string? cookieValue)
        {
            return requestCookies.TryGetValue(_settings.CookieName!, out cookieValue);
        }

        private static string GetNewCookieValue()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
