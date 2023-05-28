using Microsoft.AspNetCore.Http;

using n3q.Tools;

namespace n3q.FrameworkTools
{
    public static class ICallbackLoggerExtensions
    {
        public static void SetTraceIdentifier(this ICallbackLogger self, HttpContext httpContext)
        {
            if (httpContext == null) return;

            if (self is MicrosoftLoggingCallbackLogger msLogger) {
                var traceId = RandomString.Alphanum(4);// httpContext.TraceIdentifier;
                if (Is.Value(traceId)) {
                    msLogger.SetValue(LogData.Key.TraceId, traceId);
                }
            }
        }
    }
}