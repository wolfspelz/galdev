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
                    logData[LogData.Key.Client] = context.HttpContext.GetRemoteIpAddressHashed();
                    logData[LogData.Key.Uri] = context.HttpContext.GetUri();
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
