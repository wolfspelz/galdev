using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace n3q.Tools
{
    public interface ICallbackLogger
    {
        void Error(Exception ex, [CallerMemberName] string context = null, [CallerFilePath] string callerFilePath = null);
        void Error(string message, [CallerMemberName] string context = null, [CallerFilePath] string callerFilePath = null);
        void Error(string message, Exception ex, [CallerMemberName] string context = null, [CallerFilePath] string callerFilePath = null);
        void Error(string message, Exception ex, IDictionary<string, object> data, [CallerMemberName] string context = null, [CallerFilePath] string callerFilePath = null);
        void Error(string message, IDictionary<string, object> data, [CallerMemberName] string context = null, [CallerFilePath] string callerFilePath = null);
        void Warning(Exception ex, [CallerMemberName] string context = null, [CallerFilePath] string callerFilePath = null);
        void Warning(string message, [CallerMemberName] string context = null, [CallerFilePath] string callerFilePath = null);
        void Warning(string message, Exception ex, [CallerMemberName] string context = null, [CallerFilePath] string callerFilePath = null);
        void Warning(string message, Exception ex, IDictionary<string, object> data, [CallerMemberName] string context = null, [CallerFilePath] string callerFilePath = null);
        void Warning(string message, IDictionary<string, object> data, [CallerMemberName] string context = null, [CallerFilePath] string callerFilePath = null);
        void User(string message, [CallerMemberName] string context = null, [CallerFilePath] string callerFilePath = null);
        void Info(string message, [CallerMemberName] string context = null, [CallerFilePath] string callerFilePath = null);
        void Info(string message, IDictionary<string, object> data, [CallerMemberName] string context = null, [CallerFilePath] string callerFilePath = null);
        void Debug(string message, [CallerMemberName] string context = null, [CallerFilePath] string callerFilePath = null);
        void Debug(string message, IDictionary<string, object> data, [CallerMemberName] string context = null, [CallerFilePath] string callerFilePath = null);
        void Verbose(string message, [CallerMemberName] string context = null, [CallerFilePath] string callerFilePath = null);
        void Verbose(string message, IDictionary<string, object> data, [CallerMemberName] string context = null, [CallerFilePath] string callerFilePath = null);
        void Flooding(string message, [CallerMemberName] string context = null, [CallerFilePath] string callerFilePath = null);
        bool IsDebug();
        bool IsVerbose();
        bool IsFlooding();
        void Generic(string level, string message, [CallerMemberName] string context = null, [CallerFilePath] string callerFilePath = null);
    }

    public interface ICallbackLogger<T> : ICallbackLogger
    {
    }

    public class LogData : Dictionary<string, object>
    {
        public static class Key
        {
            public const string Arguments = "args";
            public const string Body = "body";
            public const string Client = "client";
            public const string Context = "context";
            public const string Culture = "culture";
            public const string Data = "data";
            public const string Exception = "ex";
            public const string LogLevel = "level";
            public const string Message = "message";
            public const string ObjectId = "id";
            public const string Query = "query";
            public const string TraceId = "trace";
            public const string Uri = "uri";
            public const string User = "user";
            public const string Values = "values";
        }

        public static string Join<T>(IDictionary<string, T> values)
        {
            return string.Join(" ", values.Select(kv => $"{kv.Key}={kv.Value}"));
        }

        public static string Join<T>(IEnumerable<T> values)
        {
            return string.Join(" ", values);
        }

        public LogData() { }
        public LogData(IEnumerable<KeyValuePair<string, object>> collection) : base(collection) { }
    }
}
