namespace GaldevWeb.Pages
{
    public class ListModel : GaldevPageModel
    {
        public TimelineIndex _timelines;
        public TimelineEntryList List = new();

        public ListModel(GaldevApp app, TimelineIndex timelines) : base(app, "List")
        {
            _timelines = timelines;
        }

        public void OnGet(int min = -1)
        {
            min = min < 0 ? Config.ListMinTextLength : min;
            var lang = GetLangFromCultureName(UiCultureName);
            Log.Info("", new LogData { [nameof(lang)] = lang });
            var timeline = _timelines.GetSeriesForLanguage(lang);
            List = timeline.GetFilteredList(entry => entry.TextLen > min);
        }
    }
}