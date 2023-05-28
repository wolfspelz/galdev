using System;
using System.Collections.Generic;

namespace n3q.Tools
{
    public class NullCallbackLogger : ICallbackLogger
    {
        public void Error(Exception ex, string context = null, string callerFilePath = null)  { }
        public void Error(string message, string context = null, string callerFilePath = null)  { }
        public void Error(string message, Exception ex, string context = null, string callerFilePath = null)  { }
        public void Error(string message, Exception ex, IDictionary<string, object> data, string context = null, string callerFilePath = null)  { }
        public void Error(string message, IDictionary<string, object> data, string context = null, string callerFilePath = null)  { }
        public void Warning(Exception ex, string context = null, string callerFilePath = null)  { }
        public void Warning(string message, string context = null, string callerFilePath = null)  { }
        public void Warning(string message, Exception ex, string context = null, string callerFilePath = null)  { }
        public void Warning(string message, Exception ex, IDictionary<string, object> data, string context = null, string callerFilePath = null)  { }
        public void Warning(string message, IDictionary<string, object> data, string context = null, string callerFilePath = null)  { }
        public void Info(string message, string context = null, string callerFilePath = null)  { }
        public void Info(string message, IDictionary<string, object> data, string context = null, string callerFilePath = null)  { }
        public void Debug(string message, string context = null, string callerFilePath = null)  { }
        public void Debug(string message, IDictionary<string, object> data, string context = null, string callerFilePath = null)  { }
        public void User(string message, string context = null, string callerFilePath = null)  { }
        public void Verbose(string message, string context = null, string callerFilePath = null)  { }
        public void Verbose(string message, IDictionary<string, object> data, string context = null, string callerFilePath = null)  { }
        public void Flooding(string message, string context = null, string callerFilePath = null)  { }
        public bool IsDebug() { return false; }
        public bool IsVerbose() { return false; }
        public bool IsFlooding() { return false; }
    }
}
