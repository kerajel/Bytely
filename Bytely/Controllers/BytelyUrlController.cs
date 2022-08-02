using Bytely.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Bytely.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BytelyUrlController : ControllerBase
    {
        private readonly IBytelyUrlService _urlService;
        private readonly IUrlValidator _urlValidator;
        private readonly ICookieProvider _cookieProvider;

        public BytelyUrlController(IBytelyUrlService urlService, IUrlValidator urlValidator, ICookieProvider cookieProvider)
        {
            _urlService = urlService;
            _urlValidator = urlValidator;
            _cookieProvider = cookieProvider;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserUrls()
        {
            _ = _cookieProvider.UserCookieExists(Request.Cookies, out var userGuid);
            var result = await _urlService.GetUserUrls(userGuid);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> GetShortUrl(string originUrl)
        {
            var isUrlValid = _urlValidator.IsUrlValid(originUrl, out var uri);
            if (!isUrlValid)
                return BadRequest($"Unable to shorten '{originUrl}'. It is not a valid url.");

            var userCookieExists = _cookieProvider.UserCookieExists(Request.Cookies, out var userGuid);
            if (!userCookieExists)
                _cookieProvider.AppendUserCookie(Response.Cookies, out userGuid);
            
            var result = await _urlService.GetBytelyUrl(uri!, userGuid);
            return Ok(result);
        }
    }
}