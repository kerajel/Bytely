using Bytely.Core.Services;

namespace UnitTests
{
    public class UrlValidatorTests
    {
        [Test]
        public void IsUrlValid_WhenUrlIsValid_ReturnsTrue()
        {
            // Arrange
            var sut = new UrlValidator();
            var url = @"www.google.com";

            // Act
            var result = sut.IsUrlValid(url, out var _);

            // Assert
            result.Should().BeTrue();
        }

        [Test]
        public void IsUrlValid_WhenUrlIsNotValid_ReturnsFalse()
        {
            // Arrange
            var sut = new UrlValidator();
            var url = @"wwwgooglecom";

            // Act
            var result = sut.IsUrlValid(url, out var _);

            // Assert
            result.Should().BeFalse();
        }

        [Test]
        public void IsUrlValid_WhenValidUrlDoesNotHaveScheme_AddsHttpScheme()
        {
            // Arrange
            var sut = new UrlValidator();
            var url = "yandex.ru";

            // Act
            _ = sut.IsUrlValid(url, out var uri);

            // Assert
            uri!.Scheme.Should().Be("http");
        }
    }
}