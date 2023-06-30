namespace GaldevWeb.Pages
{
    public class IndexModel : GaldevPageModel
    {
        public TimelineIndex _timelines;
        public LinkGenerator Links;
        public TimelineSeries Entries = new();

        public IndexModel(GaldevApp app, TimelineIndex timelines, LinkGenerator linkGenerator) : base(app, "Index")
        {
            _timelines = timelines;
            Links = linkGenerator;
        }

        public void OnGet()
        {
            var lang = GetLangFromCultureName(UiCultureName);
            var timeline = _timelines.GetSeriesForLanguageWithFilter(lang, entry => entry.TextLen > Config.ListMinTextLength);

            Log.Info("Index", new LogData { [nameof(lang)] = lang });
            Entries = timeline.GetAllEntries();
        }
    }
}