using System.Data;

namespace ASP_spr321.Services.Timestamp
{
    public class UnixTimestampService : ITimestampService
    {
        public long Timestamp => (DateTimeOffset.UtcNow).ToUnixTimeSeconds();
    }
}
