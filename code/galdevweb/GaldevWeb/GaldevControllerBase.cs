using Microsoft.AspNetCore.Mvc;

namespace GaldevWeb
{
    public class GaldevControllerBase : Controller
    {
        public GaldevApp App;
        public ICallbackLogger Log;
        public GaldevConfig Config;
        public string UiCultureName { get; set; }

        public GaldevControllerBase(GaldevApp app)
        {
            App = app;
            Log = App.Log;
            Config = App.Config;
            UiCultureName = Thread.CurrentThread.CurrentUICulture.Name;
        }
    }
}
