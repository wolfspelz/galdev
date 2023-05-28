using System;

namespace n3q.Tools
{
    public class ManualTime : ITimeProvider
    {
        public DateTime Time { get; set; } = DateTime.UtcNow;
        public DateTime UtcNow() => Time;
    }
}
