using System;
using System.Collections.Generic;
using System.Linq;

namespace n3q.Tools
{
    public static class ExceptionExtensions
    {
        public static List<string> GetMessages(this Exception self)
        {
            var ex = self;

            var messages = new List<string>();
            var previousMessage = "";
            while (ex is AggregateException) {
                ex = ex.InnerException;
            }
            while (ex != null) {
                if (ex.Message != previousMessage) {
                    previousMessage = ex.Message;
                    messages.Add(ex.Message);
                }
                ex = ex.InnerException;
            }
            return messages;
        }

        public static Dictionary<string, string> GetInnerExceptionDetail(this Exception self)
        {
            var ex = self;

            var result = new Dictionary<string, string>();

            if (self.InnerException != null) {
                ex = self.InnerException;
            }

            if (ex.Source != null) { result.Add("Source", ex.Source); }
            if (ex.StackTrace != null) { result.Add("Stack", ex.StackTrace.Replace(Environment.NewLine, "\\n")); }
            if (ex.TargetSite != null) { result.Add("TargetSite", ex.TargetSite.ToString()); }

            return result;
        }

        public static string ExceptionDetail(this Exception self)
        {
            var ex = self;

            return ex.GetType().FullName + 
                " | " + string.Join(" | ", ex.GetMessages().ToArray()) + 
                " | " + string.Join(" | ", ex.GetInnerExceptionDetail().Select(kv => $"{kv.Key}={kv.Value}"));
        }

    }
}
