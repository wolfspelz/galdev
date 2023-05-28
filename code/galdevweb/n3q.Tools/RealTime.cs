using System;

namespace n3q.Tools
{
    public class RealTime : ITimeProvider
    {
        public DateTime UtcNow()
        {
            return DateTime.UtcNow;
        }
    }
}
