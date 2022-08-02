using Bytely.Core.Services;
using Bytely.Models.Settings;
using Microsoft.Extensions.Options;

namespace UnitTests
{
    public class ShortUrlConverterTests
    {
        [Test]
        public void TryConvertUriLocalPathToId_WhenPathIsValidHex_ReturnsConvertedDecimal()
        {
            // Arrange
            var options = Substitute.For<IOptions<UrlProviderSettings>>();
            options.Value.Returns(new UrlProviderSettings());
            var sut = new ShortUrlConverter(options);

            var uriLocalPath = @"3E7";

            // Act
            var result = sut.TryConvertUriLocalPathToId(uriLocalPath, out var urlId);

            // Assert
            result.Should().BeTrue();
            urlId.Should().Be(999);
        }

        [Test]
        public void TryConvertUriLocalPathToId_WhenPathIsNotValidHex_ReturnsFalse()
        {
            // Arrange
            var options = Substitute.For<IOptions<UrlProviderSettings>>();
            options.Value.Returns(new UrlProviderSettings());
            var sut = new ShortUrlConverter(options);

            var uriLocalPath = @"-01";

            // Act
            var result = sut.TryConvertUriLocalPathToId(uriLocalPath, out var _);

            // Assert
            result.Should().BeFalse();
        }

        [Test]
        public void ConvertIdToShortUrl_WhenInvoked_ReturnsExpectedResults()
        {
            // Arrange
            var options = Substitute.For<IOptions<UrlProviderSettings>>();
            options.Value.Returns(new UrlProviderSettings() { UrlPort = 123, UrlTemplate = "https://localhost:{0}/{1}" });
            var sut = new ShortUrlConverter(options);

            var id = 40127845;

            // Act
            var result = sut.ConvertIdToShortUrl(id);

            // Assert
            result.Should().Be("https://localhost:123/2644D65");
        }
    }
}