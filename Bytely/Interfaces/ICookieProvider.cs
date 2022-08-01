using System.Net;

namespace Bytely.Core.Interfaces
{
    public interface ICookieProvider
    {
        void AppendUserCookie(IResponseCookies responseCookies, out string? cookieValue);

        bool UserCookieExists(IRequestCookieCollection cookieCollection, out string? cookieValue);
    }
}