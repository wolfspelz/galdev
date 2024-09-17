using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;

namespace n3q.Tools
{
    public class ConsoleCallbackLogger : ICallbackLogger
    {
        public void Error(Exception ex, string context = null, string callerFilePath = null) { DoLog(context, callerFilePath, ex.ExceptionDetail()); }
        public void Error(string message, string context = null, string callerFilePath = null) { DoLog(context, callerFilePath, message); }
        public void Error(string message, Exception ex, string context = null, string callerFilePath = null) { DoLog(context, callerFilePath, message + " " + ex.ExceptionDetail()); }
        public void Error(string message, Exception ex, IDictionary<string, object> data, string context = null, string callerFilePath = null) { DoLog(context, callerFilePath, message + " " + ex.ExceptionDetail()); }
        public void Error(string message, IDictionary<string, object> data, string context = null, string callerFilePath = null) { DoLog(context, callerFilePath, message); }
        public void Warning(Exception ex, string context = null, string callerFilePath = null) { DoLog(context, callerFilePath, ex.ExceptionDetail()); }
        public void Warning(string message, string context = null, string callerFilePath = null) { DoLog(context, callerFilePath, message); }
        public void Warning(string message, Exception ex, string context = null, string callerFilePath = null) { DoLog(context, callerFilePath, message + " " + ex.ExceptionDetail()); }
        public void Warning(string message, Exception ex, IDictionary<string, object> data, string context = null, string callerFilePath = null) { DoLog(context, callerFilePath, message + " " + ex.ExceptionDetail()); }
        public void Warning(string message, IDictionary<string, object> data, string context = null, string callerFilePath = null) { DoLog(context, callerFilePath, message); }
        public void Info(string message, string context = null, string callerFilePath = null) { DoLog(context, callerFilePath, message); }
        public void Info(string message, IDictionary<string, object> data, string context = null, string callerFilePath = null) { DoLog(context, callerFilePath, message); }
        public void Debug(string message, string context = null, string callerFilePath = null) { DoLog(context, callerFilePath, message); }
        public void Debug(string message, IDictionary<string, object> data, string context = null, string callerFilePath = null) { DoLog(context, callerFilePath, message); }
        public void User(string message, string context = null, string callerFilePath = null) { DoLog(context, callerFilePath, message); }
        public void Verbose(string message, string context = null, string callerFilePath = null) { DoLog(context, callerFilePath, message); }
        public void Verbose(string message, IDictionary<string, object> data, string context = null, string callerFilePath = null) { DoLog(context, callerFilePath, message); }
        public void Flooding(string message, string context = null, string callerFilePath = null) { DoLog(context, callerFilePath, message); }
        public bool IsDebug() { return true; }
        public bool IsVerbose() { return true; }
        public bool IsFlooding() { return true; }
        public void Generic(string level, string message, string context = null, string callerFilePath = null) { DoLog(context, callerFilePath, level + " " + message); }

        private void DoLog(string context, string callerFilePath, string message)
        {
            context = GetContext(context, callerFilePath);
            Console.WriteLine($"{context} {message}");
        }

        private static string MethodName(int skip = 0)
        {
            var st = new StackTrace();
            var sf = st.GetFrame(1 + skip);
            if (sf != null) {
                var mb = sf.GetMethod();
                if (mb != null) {
                    return mb.Name;
                }
            }
            return "<unknown>";
        }

        private static string GetContext(string context, string callerFilePath)
        {
            var result = string.IsNullOrEmpty(callerFilePath) ? "" : Path.GetFileNameWithoutExtension(callerFilePath);
            result += (string.IsNullOrEmpty(result) ? "" : ".") + (context ?? MethodName(3));
            return result;
        }
    }
}
