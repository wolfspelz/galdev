﻿using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GaldevWeb
{
    public class AppPageModel : PageModel
    {
        public MyApp App;
        public ICallbackLogger Log;
        public MyConfig Config;
        public string UiCultureName { get; set; }
        public ITextProvider I18n;

        public AppPageModel(MyApp app, string textName)
        {
            App = app;
            Log = App.Log;
            Config = App.Config;
            UiCultureName = Thread.CurrentThread.CurrentUICulture.Name;
            I18n = new TextProvider(new ReadonlyFileDataProvider(), App.Config.AppName, UiCultureName, textName);
        }
    }
}