﻿using Microsoft.AspNetCore.Mvc.RazorPages;

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
    }
}
