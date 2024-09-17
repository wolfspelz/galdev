#nullable disable
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace n3q.FrameworkTools
{
    public class MicrosoftLoggingCallbackLogger : ICallbackLogger
    {
        readonly ILogger _frameworkLogger;
        Dictionary<string, object> _values = new LogData();

        public MicrosoftLoggingCallbackLogger(ILogger frameworkLogger)
        {
            _frameworkLogger = frameworkLogger;
        }

        internal enum Level
        {
            Silent,
            Error,
            Warning,
            User,
            Info,
            Debug,
            Verbose,
            Flooding,
        }

        public void Error(Exception ex, [CallerMemberName] string context = null, [CallerFilePath] string callerFilePath = null) { DoLog(Level.Error, context, callerFilePath, "Exception", GetData(ex)); }
        public void Error(string message, [CallerMemberName] string context = null, [CallerFilePath] string callerFilePath = null) { DoLog(Level.Error, context, callerFilePath, message, NoData); }
        public void Error(string message, Exception ex, [CallerMemberName] string context = null, string callerFilePath = null) { DoLog(Level.Error, context, callerFilePath, message, GetData(ex)); }
        public void Error(string message, Exception ex, IDictionary<string, object> data, [CallerMemberName] string context = null, string callerFilePath = null) { DoLog(Level.Error, context, callerFilePath, message, GetData(ex, data)); }
        public void Error(string message, IDictionary<string, object> data, [CallerMemberName] string context = null, [CallerFilePath] string callerFilePath = null) { DoLog(Level.Error, context, callerFilePath, message, data); }
        public void Warning(Exception ex, [CallerMemberName] string context = null, [CallerFilePath] string callerFilePath = null) { DoLog(Level.Warning, context, callerFilePath, "Exception", GetData(ex)); }
        public void Warning(string message, [CallerMemberName] string context = null, [CallerFilePath] string callerFilePath = null) { DoLog(Level.Warning, context, callerFilePath, message, NoData); }
        public void Warning(string message, Exception ex, [CallerMemberName] string context = null, string callerFilePath = null) { DoLog(Level.Warning, context, callerFilePath, message, GetData(ex)); }
        public void Warning(string message, Exception ex, IDictionary<string, object> data, [CallerMemberName] string context = null, string callerFilePath = null) { DoLog(Level.Warning, context, callerFilePath, message, GetData(ex, data)); }
        public void Warning(string message, IDictionary<string, object> data, [CallerMemberName] string context = null, [CallerFilePath] string callerFilePath = null) { DoLog(Level.Warning, context, callerFilePath, message, data); }
        public void Debug(string message, [CallerMemberName] string context = null, [CallerFilePath] string callerFilePath = null) { DoLog(Level.Debug, context, callerFilePath, message, NoData); }
        public void Debug(string message, IDictionary<string, object> data, [CallerMemberName] string context = null, [CallerFilePath] string callerFilePath = null) { DoLog(Level.Debug, context, callerFilePath, message, data); }
        public void User(string message, [CallerMemberName] string context = null, [CallerFilePath] string callerFilePath = null) { DoLog(Level.User, context, callerFilePath, message, NoData); }
        public void Info(string message, [CallerMemberName] string context = null, [CallerFilePath] string callerFilePath = null) { DoLog(Level.Info, context, callerFilePath, message, NoData); }
        public void Info(string message, IDictionary<string, object> data, [CallerMemberName] string context = null, [CallerFilePath] string callerFilePath = null) { DoLog(Level.Info, context, callerFilePath, message, data); }
        public void Verbose(string message, [CallerMemberName] string context = null, [CallerFilePath] string callerFilePath = null) { DoLog(Level.Verbose, context, callerFilePath, message, NoData); }
        public void Verbose(string message, IDictionary<string, object> data, [CallerMemberName] string context = null, [CallerFilePath] string callerFilePath = null) { DoLog(Level.Verbose, context, callerFilePath, message, data); }
        public void Flooding(string message, [CallerMemberName] string context = null, [CallerFilePath] string callerFilePath = null) { DoLog(Level.Flooding, context, callerFilePath, message, NoData); }
        public bool IsDebug() { return true; }
        public bool IsVerbose() { return true; }
        public bool IsFlooding() { return true; }
        public void Generic(string level, string message, [CallerMemberName] string context = null, [CallerFilePath] string callerFilePath = null) { DoLog(LevelFromString(level), context, callerFilePath, message, NoData); }

        public readonly LogData NoData = new LogData();

        Level LevelFromString(string level)
        {
            switch (level.ToLowerInvariant()) {
                case "silent": return Level.Silent;
                case "error": return Level.Error;
                case "warning": return Level.Warning;
                case "user": return Level.User;
                case "info": return Level.Info;
                case "debug": return Level.Debug;
                case "verbose": return Level.Verbose;
                case "flooding": return Level.Flooding;
                default: return Level.Info;
            }
        }

        void DoLog(Level level, string context, string callerFilePath, string message, IDictionary<string, object> data)
        {
            try {
                context = GetContext(context, callerFilePath);

                var root = new JsonPath.Node(JsonPath.Node.Type.Dictionary);

                root[LogData.Key.Context] = context;

                if (!string.IsNullOrEmpty(message)) {
                    root[LogData.Key.Message] = message;
                }

                if (data != null && data.Count > 0) {
                    root[LogData.Key.Data] = JsonPath.Node.From(data);
                }

                if (_values != null && _values.Count > 0) {
                    root[LogData.Key.Values] = JsonPath.Node.From(_values);
                }

                var text = root.ToJson();

                switch (level) {
                    case Level.Silent:
                        break;
                    case Level.Error:
                        _frameworkLogger.LogError(text);
                        break;
                    case Level.Warning:
                        _frameworkLogger.LogWarning(text);
                        break;
                    case Level.User:
                        _frameworkLogger.LogInformation(text);
                        break;
                    case Level.Info:
                        _frameworkLogger.LogInformation(text);
                        break;
                    case Level.Debug:
                        _frameworkLogger.LogDebug(text);
                        break;
                    case Level.Verbose:
                        _frameworkLogger.LogInformation(text);
                        break;
                    case Level.Flooding:
                        _frameworkLogger.LogTrace(text);
                        break;
                }

            } catch (Exception) {
                // ignore
            }
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

        private LogData GetData(Exception ex, IDictionary<string, object> data = null)
        {
            var result = new LogData {
                [LogData.Key.Exception] = ex.ExceptionDetail(),
            };

            if (data != null) {
                result[LogData.Key.Data] = data;
            }

            return result;
        }

        public void SetValue(string key, object value)
        {
            lock (_values) {
                _values[key] = value;
            }
        }

        public void DeleteValue(string key)
        {
            lock (_values) {
                _values.Remove(key);
            }
        }

        public LogData GetValues()
        {
            lock (_values) {
                return new LogData(_values);
            }
        }

    }
}