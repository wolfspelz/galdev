using System;

namespace n3q.Tools
{
    public interface ITimeProvider
    {
        DateTime UtcNow();
    }
}
