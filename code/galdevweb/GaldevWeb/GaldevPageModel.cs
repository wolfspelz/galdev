using Microsoft.AspNetCore.Mvc.RazorPages;
using JsonPath;
using ICallbackLogger = n3q.Tools.ICallbackLogger;
using Microsoft.AspNetCore.Mvc.Filters;
using n3q.FrameworkTools;

namespace GaldevWeb
{
    public class GaldevPageModel : PageModel
    {
        public GaldevApp App;
        public ICallbackLogger Log;
        public GaldevConfig Config;
        public TimelineSeries Timeline;

        public string UiCultureName;
        public ITextProvider I18n;
        public string Lang => GetLangFromCultureName(UiCultureName);
        public CarouselModel Carousel;

        public GaldevPageModel(GaldevApp app, string textName)
        {
            App = app;
            Log = App.Log;
            Config = App.Config;
            UiCultureName = Thread.CurrentThread.CurrentUICulture.Name;
            I18n = new TextProvider(new ReadonlyFileDataProvider(), App.Config.AppName, UiCultureName, textName);

            Timeline = App.Timelines.GetSeriesForLanguage(Lang);

            Carousel = new CarouselModel();
            Carousel.Load(Timeline);
        }

        public static string GetLangFromCultureName(string cultureName)
        {
            return cultureName.Split(new char[] { '-', '_' }).First().ToLower();
        }

        public override async Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context, PageHandlerExecutionDelegate next)
        {
            try {
                var logData = new LogData();

                if (context.HandlerArguments.Count > 0) {
                    logData[LogData.Key.Arguments] = string.Join(" ", context.HandlerArguments.Select(kv => $"{kv.Key}={kv.Value}"));
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

                Log.Info($"Page", logData, context.ActionDescriptor.DisplayName, "");
            } catch (Exception ex) {
                Log.Error(ex);
            }

            await next.Invoke();
        }
    }
}
