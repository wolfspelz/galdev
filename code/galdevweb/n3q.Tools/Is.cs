using System;

namespace n3q.Tools
{
    public static class Is
    {
        public static bool Value(string self)
        {
            return !string.IsNullOrEmpty(self);
        }

        public static bool Empty(string self)
        {
            return string.IsNullOrEmpty(self);
        }

        public static bool True(string self)
        {
            return Is.Value(self) && self.IsTrue();
        }

        public static bool Thing(object self)
        {
            return self != null;
        }

    }
}
