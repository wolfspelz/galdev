using System;
using System.Collections.Generic;
using System.Text;

namespace galdevtool
{
    public class UtcTimeSource : ITimeSource
    {
        public DateTime Now()
        {
            return DateTime.UtcNow;
        }
    }

    public class LocalTimeSource : ITimeSource
    {
        public DateTime Now()
        {
            return DateTime.Now;
        }
    }
}
