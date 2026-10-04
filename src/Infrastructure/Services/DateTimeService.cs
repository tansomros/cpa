using BigLion.CPA.Application.Common.Interfaces;

namespace BigLion.CPA.Infrastructure.Services
{
    public class DateTimeService : IDateTime
    {
        public DateTimeOffset Now => DateTimeOffset.UtcNow;
    }
}
