using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace galdevtool
{
    public class GlobalCallbackLogger : ICallbackLogger
    {
        public string Class { get; set; }

        public GlobalCallbackLogger(string className)
        {
            Class = className;
        }

        public void Error(Exception ex, [CallerMemberName] string context = null) { _Log("Error", CombinedContext(context), ExceptionDetail(ex)); }
        public void Error(string message, [CallerMemberName] string context = null) { _Log("Error", CombinedContext(context), message); }
        public void Warning(Exception ex, [CallerMemberName] string context = null) { _Log("Warning", CombinedContext(context), ExceptionDetail(ex)); }
        public void Warning(string message, [CallerMemberName] string context = null) { _Log("Warning", CombinedContext(context), message); }
        public void Debug(string message, [CallerMemberName] string context = null) { _Log("Debug", CombinedContext(context), message); }
        public void User(string message, [CallerMemberName] string context = null) { _Log("User", CombinedContext(context), message); }
        public void Info(string message, [CallerMemberName] string context = null) { _Log("Info", CombinedContext(context), message); }
        public void Verbose(string message, [CallerMemberName] string context = null) { _Log("Verbose", CombinedContext(context), message); }
        public void Flooding(string message, [CallerMemberName] string context = null) { _Log("Flooding", CombinedContext(context), message); }
        public bool IsVerbose() { return Log.IsVerbose; }
        public bool IsFlooding() { return Log.IsFlooding; }
        public void _Log(string level, string context, string message)
        {
            Log._Log(Log.LevelFromString(level), context, message);
        }

        private string CombinedContext(string context)
        {
            return Class + " " + context;
        }

        private static string ExceptionDetail(Exception ex)
        {
            return string.Join(" | ", AllExceptionMessages(ex).ToArray()) + " | " + string.Join(" | ", InnerExceptionDetail(ex).ToArray());
        }

        private static List<string> AllExceptionMessages(Exception self)
        {
            var result = new List<string>();

            var ex = self;
            var previousMessage = "";
            while (ex != null)
            {
                if (ex.Message != previousMessage)
                {
                    previousMessage = ex.Message;
                    result.Add(ex.Message);
                }
                ex = ex.InnerException;
            }

            return result;
        }

        private static List<string> InnerExceptionDetail(Exception self)
        {
            var result = new List<string>();

            var ex = self;
            if (self.InnerException != null)
            {
                ex = self.InnerException;
            }

            if (ex.Source != null) { result.Add("Source: " + ex.Source); }
            if (ex.StackTrace != null) { result.Add("Stack Trace: " + ex.StackTrace.Replace(Environment.NewLine, "\\n")); }
            if (ex.TargetSite != null) { result.Add("TargetSite: " + ex.TargetSite); }

            return result;
        }
    }

    public class GlobalCallbackLogger<T> : GlobalCallbackLogger
    {
        public GlobalCallbackLogger() : base(typeof(T).Name) { }
    }
}
