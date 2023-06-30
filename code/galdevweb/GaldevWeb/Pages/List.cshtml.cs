namespace GaldevWeb.Pages
{
    public class ListModel : GaldevPageModel
    {
        public TimelineIndex _timelines;
        public List<TimelineEntry> List = new();

        public ListModel(GaldevApp app, TimelineIndex timelines) : base(app, "List")
        {
            _timelines = timelines;
        }

        public void OnGet()
        {
            var lang = GetLangFromCultureName(UiCultureName);
            Log.Info("", new LogData { [nameof(lang)] = lang });
            var timeline = _timelines.GetSeriesForLanguage(lang);
            List = timeline.GetFilteredList(entry => entry.TextLen > Config.ListMinTextLength);
        }
    }
}