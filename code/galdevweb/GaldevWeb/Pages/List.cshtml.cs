namespace GaldevWeb.Pages
{
    public class ListModel : GaldevPageModel
    {
        public TimelineIndex _timelines;
        public List<TimelineEntry> Entries = new();

        public ListModel(GaldevApp app, TimelineIndex timelines) : base(app, "List")
        {
            _timelines = timelines;
        }

        public void OnGet()
        {
            var lang = GetLangFromCultureName(UiCultureName);
            Log.Info("List", new LogData { [nameof(lang)] = lang });
            var timeline = _timelines.GetSeriesForLanguage(lang);
            Entries = timeline.GetFilteredList(entry => entry.TextLen > Config.ListMinTextLength);
        }
    }
}