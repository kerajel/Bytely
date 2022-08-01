using Bytely.Core.Interfaces;

namespace Bytely.Core.Services
{
    public class UrlValidator : IUrlValidator
    {
        public bool IsUrlValid(string url, out Uri? uri)
        {
            var period = url.IndexOf(".");
            if (period > -1 && !url.Contains('@'))
            {
                var colon = url.IndexOf(":");
                var slash = url.IndexOf("/");
                if ((colon == -1 || period < colon) &&
                    (slash == -1 || period < slash))
                {
                    url = $"http://{url}";
                }
            }

            var result = Uri.TryCreate(url, UriKind.Absolute, out uri)
                && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
            return result;
        }
    }
}
