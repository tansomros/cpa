using Cpa.Application.Common.Interfaces;

namespace Cpa.Infrastructure.Services
{
    public class DateTimeService : IDateTime
    {
        public DateTimeOffset Now => DateTimeOffset.UtcNow;
    }
}
