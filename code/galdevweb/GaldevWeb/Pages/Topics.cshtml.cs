namespace GaldevWeb.Pages
{
    public class TopicsModel : GaldevPageModel
    {
        public TimelineIndex _timelines;
        public LinkGenerator Links;
        public TimelineSeries Series = new();

        public TopicsModel(GaldevApp app, TimelineIndex timelines, LinkGenerator linkGenerator) : base(app, "Topics")
        {
            _timelines = timelines;
            Links = linkGenerator;
        }

        public void OnGet()
        {
            var lang = GetLangFromCultureName(UiCultureName);
            Log.Info("", new LogData { [nameof(lang)] = lang });
            Series = _timelines.GetSeriesForLanguage(lang);
        }
    }
}