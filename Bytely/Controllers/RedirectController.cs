using Bytely.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Bytely.Controllers
{
    [ApiController]
    [Route("")]
    public class RedirectController : ControllerBase
    {
        private readonly IBytelyUrlService _urlService;
        private readonly IShortUrlConvertor _urlConvertor;

        public RedirectController(IBytelyUrlService urlService, IShortUrlConvertor urlConvertor)
        {
            _urlService = urlService;
            _urlConvertor = urlConvertor;
        }

        [HttpGet("{uriLocalPath}")]
        public async Task<IActionResult> RedirectToOriginUrl(string uriLocalPath)
        {
           var isUrlPathConverted = _urlConvertor.TryConvertUriLocalPathToId(uriLocalPath, out var urlId);

            if (!isUrlPathConverted)
                return NotFound();

            var originUrl = await _urlService.GetOriginUrl(urlId);

            if (originUrl is null)
                return NotFound();
            
            return Redirect(originUrl);
        }
    }
}