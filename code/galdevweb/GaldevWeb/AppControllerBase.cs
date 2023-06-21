using Microsoft.AspNetCore.Mvc;

namespace GaldevWeb
{
    public class AppControllerBase : Controller
    {
        public MyApp App;
        public ICallbackLogger Log;
        public MyConfig Config;

        public AppControllerBase(MyApp app)
        {
            App = app;
            Log = App.Log;
            Config = App.Config;
        }
    }
}
