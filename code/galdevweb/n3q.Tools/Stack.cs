using System;

namespace n3q.Tools
{
    public static class Stack
    {
        public static string GetMethodName(int level = 0, bool fullName = false)
        {
            var st = new System.Diagnostics.StackTrace();
            var sf = st.GetFrame(1 + level);
            var mb = sf.GetMethod();
            if (fullName) {
                return mb.DeclaringType.FullName + "." + mb.Name;
            }
            return mb.Name;
        }

        public static string GetCallerName()
        {
            return GetMethodName(2);
        }
    }
}
