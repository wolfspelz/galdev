namespace n3q.FrameworkTools;

public static class ICallbackLoggerExtensions
{
    public static void SetTraceIdentifier(this ICallbackLogger self, HttpContext httpContext)
    {
        if (self is MicrosoftLoggingCallbackLogger msLogger) {
            var traceId = httpContext.TraceIdentifier; // RandomString.Alphanum(4);// httpContext.TraceIdentifier;
            if (Is.Value(traceId)) {
                msLogger.SetValue(LogData.Key.TraceId, traceId);
            }
        }
    }
}
