
using System.Data;

namespace ASP_spr321.Services.Timestamp
{
    public class SystemTimestampService : ITimestampService
    {
        public long Timestamp=>DateTime.Now.Ticks;
    }
}
