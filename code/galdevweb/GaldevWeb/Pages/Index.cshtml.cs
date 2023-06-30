namespace GaldevWeb.Pages
{
    public class IndexModel : GaldevPageModel
    {
        public TimelineIndex _timelines;
        public LinkGenerator Links;
        public TimelineSeries Series = new();

        public IndexModel(GaldevApp app, TimelineIndex timelines, LinkGenerator linkGenerator) : base(app, "Index")
        {
            _timelines = timelines;
            Links = linkGenerator;
        }

        public void OnGet()
        {
            var lang = GetLangFromCultureName(UiCultureName);
            Log.Info("Index", new LogData { [nameof(lang)] = lang });
            Series = _timelines.GetSeriesForLanguage(lang);
        }
    }
}