using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using n3q.FrameworkTools;

namespace GaldevWeb
{
    public class GaldevControllerBase : Controller
    {
        public GaldevApp App;
        public ICallbackLogger Log;
        public GaldevConfig Config;
        public TimelineIndex Timelines;
        public string UiCultureName;

        public GaldevControllerBase(GaldevApp app)
        {
            App = app;
            Log = App.Log;
            Config = App.Config;
            Timelines = App.Timelines;
            UiCultureName = Thread.CurrentThread.CurrentUICulture.Name;
        }

        [NonAction]
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            try {
                var logContext = context.ActionDescriptor.DisplayName;
                if (context.ActionDescriptor is ControllerActionDescriptor cad) {
                    logContext = $"{cad.ControllerName}.{cad.ActionName}";
                }

                var logData = new LogData();
                if (context.ActionArguments.Count > 0) {
                    logData[LogData.Key.Arguments] = string.Join(" ", context.ActionArguments.Select(kv => $"{kv.Key}={kv.Value}"));
                }

                logData[LogData.Key.Culture] = UiCultureName;

                if (context.HttpContext != null) {

                    var ipAddress = context.HttpContext.Connection?.RemoteIpAddress?.ToString();
                    if (Is.Value(ipAddress)) {
                        var hashedIp = Crc32.Compute(ipAddress).ToString("X8");
                        logData[LogData.Key.Client] = hashedIp;
                    }

                    var request = context.HttpContext.Request;
                    var uri = request?.Path.Value + request?.QueryString.Value;
                    if (Is.Value(uri)) {
                        logData[LogData.Key.Uri] = uri;
                    }

                    Log.SetTraceIdentifier(context.HttpContext);
                }

                Log.Info($"Controller", logData, logContext, "");
            } catch (Exception ex) {
                Log.Error(ex);
            }

            base.OnActionExecuting(context);
        }
    }
}
