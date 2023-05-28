using System;

namespace n3q.Tools
{
    public static class Don
    {
        // Compile & refactor, but do not execute and don't complain about unreachable statement like "if (false)"
        public static Action t { get; set; }
        /*
                Don.t = () => {
                };
        */
    }
}
