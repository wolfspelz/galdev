using System;
using System.Runtime.CompilerServices;

namespace galdevtool
{
    public interface ICallbackLogger
    {
        void Error(Exception ex, [CallerMemberName] string context = null);
        void Error(string message, [CallerMemberName] string context = null);
        void Warning(Exception ex, [CallerMemberName] string context = null);
        void Warning(string message, [CallerMemberName] string context = null);
        void Debug(string message, [CallerMemberName] string context = null);
        void User(string message, [CallerMemberName] string context = null);
        void Info(string message, [CallerMemberName] string context = null);
        void Verbose(string message, [CallerMemberName] string context = null);
        void Flooding(string message, [CallerMemberName] string context = null);
        bool IsVerbose();
        bool IsFlooding();
        void _Log(string level, string context, string message);
    }

    public interface ICallbackLogger<T> : ICallbackLogger
    {
    }

    public class NullCallbackLogger : ICallbackLogger
    {
        public void Debug(string message, string context = null) { }
        public void Error(Exception ex, string context = null) { }
        public void Error(string message, string context = null) { }
        public void Flooding(string message, string context = null) { }
        public void Info(string message, string context = null) { }
        public void User(string message, string context = null) { }
        public void Verbose(string message, string context = null) { }
        public void Warning(Exception ex, string context = null) { }
        public void Warning(string message, string context = null) { }
        public bool IsFlooding() { return false; }
        public bool IsVerbose() { return false; }
        public void _Log(string level, string context, string message) { }
    }
}
