namespace GaldevWeb.Pages
{
    public class IndexModel : GaldevPageModel
    {
        public TimelineIndex _timelines;
        public TimelineSeries Series = new();

        public IndexModel(GaldevApp app, TimelineIndex timelines) : base(app, "Index")
        {
            _timelines = timelines;
        }

        public void OnGet()
        {
            var lang = GetLangFromCultureName(UiCultureName);
            Log.Info("", new LogData { [nameof(lang)] = lang });
            Series = _timelines.GetSeriesForLanguage(lang);
        }
    }
}