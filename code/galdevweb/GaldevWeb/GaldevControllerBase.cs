using Microsoft.AspNetCore.Mvc;

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
    }
}
