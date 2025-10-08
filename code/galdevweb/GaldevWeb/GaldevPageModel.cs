using Microsoft.AspNetCore.Mvc.RazorPages;
using JsonPath;
using ICallbackLogger = n3q.Tools.ICallbackLogger;
using Microsoft.AspNetCore.Mvc.Filters;
using n3q.FrameworkTools;

namespace GaldevWeb;

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
                logData[LogData.Key.Client] = context.HttpContext.GetRemoteIpAddressHashed();

                var uri = context.HttpContext.GetUri();
                if (Is.Value(uri)) {
                    logData[LogData.Key.Uri] = uri;
                }

                var ua = context.HttpContext.GetUserAgent();
                if (Is.Value(ua)) {
                    logData[LogData.Key.Agent] = ua;
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
