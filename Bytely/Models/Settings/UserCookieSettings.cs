namespace Bytely.Models.Settings
{
    public class UserCookieSettings
    {
        public string? CookieName { get; set; }
        public TimeSpan ExpiresIn { get; set; }
    }
}
