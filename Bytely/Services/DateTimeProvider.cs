using Bytely.Core.Interfaces;

namespace Bytely.Core.Services
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
