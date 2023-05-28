using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GaldevWeb
{
    public class AppPageModel : PageModel
    {
        public MyApp App;
        public ICallbackLogger Log;
        public MyConfig Config;
        public ITextProvider I18n;

        public AppPageModel(MyApp app, string textName)
        {
            App = app;
            Log = App.Log;
            Config = App.Config;
            I18n = new TextProvider(new ReadonlyFileDataProvider(), App.Config.AppName, Thread.CurrentThread.CurrentUICulture.Name, textName);
        }
    }
}
