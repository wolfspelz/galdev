namespace GaldevWeb.Pages
{
    public class ListModel : GaldevPageModel
    {
        public TimelineIndex _timelines;
        public TimelineSeries Entries = new TimelineSeries();

        public ListModel(GaldevApp app, TimelineIndex timelines) : base(app, "List")
        {
            _timelines = timelines;
        }

        public void OnGet(string name)
        {
            var lang = GetLangFromCultureName(UiCultureName);
            var timeline = _timelines.GetEntries(lang, entry => entry.TextLen > Config.ListMinTextLength);

            Log.Info("List", new LogData { [nameof(lang)] = lang });
            Entries = timeline.GetAllEntries();
        }
    }
}